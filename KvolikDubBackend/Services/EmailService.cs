using System.Text.RegularExpressions;
using KvolikDubBackend.Exceptions;
using KvolikDubBackend.Models;
using KvolikDubBackend.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
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

//todo: доделать + плеер и трейлеры
    public void SendCodeToEmail(string emailAddress)
    {
        Regex regex = new Regex(@"[a-zA-Z]+\w*@[a-zA-Z]+\.[a-zA-Z]+");
        MatchCollection matches = regex.Matches(emailAddress);
        if (matches.Count == 0)
        {
            throw new BadRequestException("Invalid email");
        }

        Random r = new Random();
        int randomCode = r.Next(1000, 9999);
        
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(corpEmail));
        email.To.Add(MailboxAddress.Parse(emailAddress));
        email.Subject = "Подтверждение позьзователя";
        email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>Ваш код {randomCode}</h1>" };
        
        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(corpEmail, corpP);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}