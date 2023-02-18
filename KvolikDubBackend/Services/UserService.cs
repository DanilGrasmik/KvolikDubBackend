using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
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

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<TokenDto> RegisterUser(UserRegisterDto userRegisterDto)
    {
         await CheckRegisterValidation(userRegisterDto);
        
        UserEntity userEntity = new UserEntity()
        {
            Id = new Guid(),
            Email = userRegisterDto.email,
            Name = userRegisterDto.name,
            AvatarImageUrl = "https://sun9-east.userapi.com/sun9-58/s/v1/ig2/wgxhVCsTLeNhC3Ue8gnd8n6QkpilZZHTxT61fUXzPXfWqjH8vTsui8fZMdX3VvBHuhMEKYZOkZPosFeMS8CzElzc.jpg?size=712x1044&quality=96&type=album",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.password),
            IsAdmin = false
        };
        
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        LoginCredentials loginCredentials = new LoginCredentials()
        {
            email = userRegisterDto.email,
            password = userRegisterDto.password
        };

        return await LoginUser(loginCredentials);
    }

    public async Task<TokenDto> LoginUser(LoginCredentials loginCredentials)
    {
        var identity = await GetIdentity(loginCredentials.email, loginCredentials.password);

        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: JwtConfig.Issuer,
            audience: JwtConfig.Audience,
            notBefore: now,
            claims: identity.Claims,
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

    public async Task<String> LogoutUser(IHeaderDictionary headers)
    {
        var token = GetToken(headers);
        
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token);
        var tokenS = jsonToken as JwtSecurityToken;
        var claims = tokenS.Claims;
        var expiredDate = handler.ReadJwtToken(token).ValidTo;

        var tokenEntity = new TokenEntity
        {
            Id = Guid.NewGuid(),
            Token = token,
            ExpiredDate = expiredDate
        };

        await _context.Tokens.AddAsync(tokenEntity);
        await _context.SaveChangesAsync();
        return "Logged out";
    }

    //todo: оценки аниме id + grade
    public async Task<ProfileInfoDto> GetProfile(string email)
    {
        var userEntity = await _context
            .Users
            .Where(user => user.Email == email)
            .FirstOrDefaultAsync();
        
        ProfileInfoDto profileInfoDto = _mapper.Map<ProfileInfoDto>(userEntity);
        return profileInfoDto;
    }

    public async Task<TokenDto> EditProfile(EditProfileDto editProfileDto, String email, IHeaderDictionary headers)
    {
        var userEntity = await _context
            .Users
            .Where(user => user.Email == email)
            .FirstOrDefaultAsync();
        var userId = userEntity.Id;
        await CheckEditValidation(editProfileDto, userId);
        
        userEntity.Name = editProfileDto.name;
        userEntity.Email = editProfileDto.email;
        userEntity.HashedPassword = BCrypt.Net.BCrypt.HashPassword(editProfileDto.password);
        await _context.SaveChangesAsync();

        await LogoutUser(headers);
        
        LoginCredentials loginCredentials = new LoginCredentials()
        {
            email = editProfileDto.email,
            password = editProfileDto.password
        };
        return await LoginUser(loginCredentials);
    }


    //auxiliary
    private async Task<ClaimsIdentity> GetIdentity(string email, string password)
    {
        var userEntity = await _context
            .Users
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync() ?? throw new BadRequestException("Login failed");

        if (!BCrypt.Net.BCrypt.Verify(password, userEntity.HashedPassword))
        {
            throw new BadRequestException("Login failed");
        }

        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, userEntity.Email),
            new("username", userEntity.Email)
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

    private async Task CheckRegisterValidation(UserRegisterDto userRegisterDto)
    {
        Regex regex = new Regex(@"[a-zA-Z]+\w*@[a-zA-Z]+\.[a-zA-Z]+");
        MatchCollection matches = regex.Matches(userRegisterDto.email);
        if (matches.Count == 0)
        {
            throw new BadRequestException("Invalid email");
        }
        
        if (userRegisterDto.password == null || userRegisterDto.email == null)
        {
            throw new BadRequestException("Incorrect model in request");
        }
        if (userRegisterDto.password.Length < 6 || userRegisterDto.password.Length > 30)
        {
            throw new BadRequestException("Password length must be in range 6 to 30");
        }
        if(userRegisterDto.name.Length < 2 || userRegisterDto.name.Length > 25)
        {
            throw new BadRequestException("Username length must be in range 2 to 25");
        }
        
        if (userRegisterDto.password != userRegisterDto.confirmPassword)
        {
            throw new BadRequestException("Confirm password not match password");
        }

        UserEntity? userEntity = await _context
            .Users
            .Where(user => user.Email == userRegisterDto.email)
            .FirstOrDefaultAsync();
        if (userEntity != null)
        {
            throw new ConflictException($"User with email '{userRegisterDto.email}' already exists");
        }
    }
    
    private static string GetToken(IHeaderDictionary headersDictionary)
    {
        var headers = new Dictionary<string, string>();

        foreach (var header in headersDictionary)
        {
            headers.Add(header.Key, header.Value);
        }

        var authorizationHeader = headers["Authorization"];

        var regex = new Regex(@"\S+\.\S+\.\S+");
        var matches = regex.Matches(authorizationHeader);

        if (matches.Count <= 0)
        {
            throw new BadRequestException("Invalid token in authorization header");
        }

        return matches[0].Value;
    }

    private async Task CheckEditValidation(EditProfileDto editProfileDto, Guid userId)
    {
        Regex regex = new Regex(@"[a-zA-Z]+\w*@[a-zA-Z]+\.[a-zA-Z]+");
        MatchCollection matches = regex.Matches(editProfileDto.email);
        if (matches.Count == 0)
        {
            throw new BadRequestException("Invalid email");
        }
        if (editProfileDto.password.Length < 6 || editProfileDto.password.Length > 30)
        {
            throw new BadRequestException("Password length must be in range 6 to 30");
        }
        if (editProfileDto.email.Length < 2 || editProfileDto.email.Length > 25)
        {
            throw new BadRequestException("Username length must be in range 2 to 25");
        }
        
        var existsUser = await _context
            .Users
            .Where(user => user.Email == editProfileDto.email && user.Id != userId)
            .FirstOrDefaultAsync();
        if (existsUser != null)
        {
            throw new ConflictException($"User with username '{editProfileDto.email}' already exists");
        }
    }
}