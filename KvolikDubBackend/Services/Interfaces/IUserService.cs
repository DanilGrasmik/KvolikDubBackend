using System.ComponentModel.DataAnnotations;
using KvolikDubBackend.Models.Dtos;

namespace KvolikDubBackend.Services.Interfaces;

public interface IUserService
{
    public Task<TokenDto> RegisterUser(UserRegisterDto userRegisterDto);
    public Task<TokenDto> LoginUser(LoginCredentials loginCredentials);
    public Task<String> LogoutUser(IHeaderDictionary headers);
    public Task<ProfileInfoDto> GetProfile(String email);
    public Task<TokenDto> EditProfile(EditProfileDto editProfileDto, String email, IHeaderDictionary headers);
}