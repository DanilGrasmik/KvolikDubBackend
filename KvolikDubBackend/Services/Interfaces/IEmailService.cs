using KvolikDubBackend.Models.Dtos;

namespace KvolikDubBackend.Services.Interfaces;

public interface IEmailService
{
    public Task SendCodeToEmail(String emailAddress);
    public Task<TokenDto> ConfirmCode(ConfirmCodeDto confirmCodeDto);
}