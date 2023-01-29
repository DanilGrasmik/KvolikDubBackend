using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KvolikDubBackend.Controllers;

[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost]
    [Route("anime")]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Authorize(Policy = "AdminRequest")]
    public async Task CreateNewAnime([FromBody] CreateAnimeDto createAnimeDto)
    {
        await _adminService.CreateAnime(createAnimeDto);
    }
    
    [HttpPut]
    [Route("anime/{animeId}")]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Authorize(Policy = "AdminRequest")]
    public async Task CreateNewAnime([FromBody] CreateAnimeDto createAnimeDto, Guid animeId)
    {
        await _adminService.EditAnime(createAnimeDto, animeId);
    }
}