using System.ComponentModel.DataAnnotations;
using KvolikDubBackend.Models.Dtos;

namespace KvolikDubBackend.Services.Interfaces;

public interface IUserService
{
    public Task<TokenDto> RegisterUser(UserRegisterDto userRegisterDto);
    public Task<TokenDto> LoginUser(LoginCredentials loginCredentials);
    public Task<String> LogoutUser(IHeaderDictionary headers);
    public Task<ProfileInfoDto> GetProfile(String username);
    public Task<TokenDto> EditProfile(EditProfileDto editProfileDto, String username, IHeaderDictionary headers);
}