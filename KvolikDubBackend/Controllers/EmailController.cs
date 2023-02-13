using KvolikDubBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KvolikDubBackend.Controllers;

[Route("api/email")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    /// <summary>
    /// Отправить код на введенную почту
    /// </summary>
    [HttpPut]
    [Route("send")]
    public void SendEmail([FromBody] String emailAddress)
    {
        _emailService.SendCodeToEmail(emailAddress);
    }
}