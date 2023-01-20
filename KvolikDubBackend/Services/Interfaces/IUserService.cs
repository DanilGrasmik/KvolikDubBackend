using KvolikDubBackend.Models.Dtos;

namespace KvolikDubBackend.Services.Interfaces;

public interface IUserService
{
    public Task<TokenDto> RegisterUser(UserRegisterDto userRegisterDto);

    public Task<TokenDto> LoginUser(LoginCredentials loginCredentials);
}