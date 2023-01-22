using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KvolikDubBackend.Controllers;

[Route("api/account")]
public class UserController : ControllerBase
{
    private readonly IUserService _usersService;
    public UserController(IUserService usersService)
    {
        _usersService = usersService;
    }

    /// <summary>
    /// Зарегистрировать пользователя
    /// </summary>
    [HttpPost]
    [Route("register")]
    public async Task<TokenDto> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        return await _usersService.RegisterUser(userRegisterDto);
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    [HttpPost]
    [Route("login")]
    public async Task<TokenDto> LoginUser([FromBody] LoginCredentials loginCredentials)
    {
        return await _usersService.LoginUser(loginCredentials);
    }

    /// <summary>
    /// Выйти из аккаунта
    /// </summary>
    [HttpPost]
    [Authorize]
    [Route("logout")]
    public async Task<String> LogoutUser()
    {
        return await _usersService.LogoutUser(HttpContext.Request.Headers);
    }

    /// <summary>
    /// Получить инф-ию о пользователе
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ProfileInfoDto> GetProfile()
    {
        return await _usersService.GetProfile(User.Identity.Name);
    }

    /// <summary>
    /// Изменить инф-ию о пользователе
    /// </summary>
    [HttpPut]
    [Authorize]
    public async Task EditProfile([FromBody] EditProfileDto editProfileDto)
    {
        await _usersService.EditProfile(editProfileDto, User.Identity.Name);
    }
}