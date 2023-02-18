using KvolikDubBackend.Models.Dtos;
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
    [Route("send/{emailAddress}")]
    public async Task SendEmail(String emailAddress)
    {
        await _emailService.SendCodeToEmail(emailAddress);
    }

    /// <summary>
    /// Подтвердить код (максимум 5 попыток)
    /// </summary>
    [HttpPut]
    [Route("confirm")]
    public async Task<TokenDto> SendEmail([FromBody] ConfirmCodeDto confirmCodeDto)
    {
        return await _emailService.ConfirmCode(confirmCodeDto);
    }
}