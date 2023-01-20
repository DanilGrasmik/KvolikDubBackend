using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KvolikDubBackend.Controllers;

[Microsoft.AspNetCore.Components.Route("api/account")]
public class UserController
{
    private readonly IUserService _usersService;
    public UserController(IUserService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<TokenDto> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        return await _usersService.RegisterUser(userRegisterDto);
    }

    [HttpPost]
    [Route("login")]
    public async Task<TokenDto> LoginUser([FromBody] LoginCredentials loginCredentials)
    {
        return await _usersService.LoginUser(loginCredentials);
    }
}