using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using KvolikDubBackend.Configurations;
using KvolikDubBackend.Exceptions;
using KvolikDubBackend.Models;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Models.Entities;
using KvolikDubBackend.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;

namespace KvolikDubBackend.Services;

public class EmailService : IEmailService
{
    private readonly AppDbContext _context;
    private readonly string? corpEmail;
    private readonly string? corpP;

    public EmailService(IConfiguration configuration, AppDbContext context)
    {
        _context = context;
        corpEmail = configuration.GetValue<String>("Email");
        corpP = configuration.GetValue<String>("Password");
    }

    public async Task SendCodeToEmail(string emailAddress)
    {
        var userEntity = await _context
            .Users
            .Where(user => user.Email == emailAddress)
            .FirstOrDefaultAsync() ?? throw new BadRequestException($"Cant find user with email {emailAddress}");
        
        Regex regex = new Regex(@"[a-zA-Z]+\w*@[a-zA-Z]+\.[a-zA-Z]+");
        MatchCollection matches = regex.Matches(emailAddress);
        if (matches.Count == 0)
        {
            throw new BadRequestException("Invalid email");
        }
        
        Random r = new Random();
        int randomCode = r.Next(1000, 9999);
        var codeEntity = new ConfirmCodeEntity()
        {
            Code = randomCode,
            UserEmail = emailAddress,
            ExpiredDate = DateTime.UtcNow.AddMinutes(ConfirmCodeConfig.Lifetime)
        };
        await _context.ConfirmCodes.AddAsync(codeEntity);
        await _context.SaveChangesAsync();
        
        SendEmail(emailAddress, randomCode);
    }
    
    public async Task<TokenDto> ConfirmCode(ConfirmCodeDto confirmCodeDto)
    {
        var codeEntity = await _context
            .ConfirmCodes
            .Where(cc => cc.Code == confirmCodeDto.code)
            .FirstOrDefaultAsync();
        if (codeEntity == null)
        {
            await AddCodeAttempt(confirmCodeDto.email);
        }

        _context.ConfirmCodes.Remove(codeEntity);
        
        return await LoginUserByEmail(codeEntity.UserEmail);
    }

    private void SendEmail(string emailAddress, int randomCode)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(corpEmail));
        email.To.Add(MailboxAddress.Parse(emailAddress));
        email.Subject = "Подтверждение позьзователя";
        email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>Ваш код {randomCode}</h1>" };
        
        using var smtp = new SmtpClient();
        smtp.Connect("smtp.timeweb.ru", 25, SecureSocketOptions.StartTls);
        smtp.Authenticate(corpEmail, corpP);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
    
    private async Task<TokenDto> LoginUserByEmail(string email)
    {
        var claims = CreateClaims(email);
        
        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: JwtConfig.Issuer,
            audience: JwtConfig.Audience,
            notBefore: now,
            claims: claims.Claims,
            expires: now.AddMinutes(JwtConfig.Lifetime),
            signingCredentials: new SigningCredentials(JwtConfig.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var result = new TokenDto()
        {
            token = encodeJwt
        };

        return result;
    }
    
    private ClaimsIdentity CreateClaims(string email)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, email),
            new("username", email)
        };

        var claimsIdentity = new ClaimsIdentity
        (
            claims,
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            "User"
        );

        return claimsIdentity;
    }

    private async Task AddCodeAttempt(string email)
    {
        var codeEntity = await _context
            .ConfirmCodes
            .Where(code => code.UserEmail == email)
            .FirstOrDefaultAsync() ?? throw new BadRequestException("Incorrect email");

        codeEntity.ConfirmAttempts++;
        if (codeEntity.ConfirmAttempts > 4)
        {
            _context.ConfirmCodes.Remove(codeEntity);
            await _context.SaveChangesAsync();
            throw new GoneException("Confirm code removed because of many attempts to confirm it");
        }

        await _context.SaveChangesAsync();

        throw new BadRequestException("incorrect code");
    }
}