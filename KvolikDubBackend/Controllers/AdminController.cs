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

    /// <summary>
    /// Создать новое аниме
    /// </summary>
    [HttpPost]
    [Route("anime")]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Authorize(Policy = "AdminRequest")]
    public async Task CreateNewAnime([FromBody] CreateAnimeDto createAnimeDto)
    {
        await _adminService.CreateAnime(createAnimeDto);
    }
    
    /// <summary>
    /// Изменить инфу о конкретном аниме
    /// </summary>
    [HttpPut]
    [Route("anime/{animeId}")]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Authorize(Policy = "AdminRequest")]
    public async Task EditNewAnime([FromBody] CreateAnimeDto createAnimeDto, Guid animeId)
    {
        await _adminService.EditAnime(createAnimeDto, animeId);
    }
    
    /// <summary>
    /// Удалить аниме
    /// </summary>
    [HttpDelete]
    [Route("anime/{animeId}")]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Authorize(Policy = "AdminRequest")]
    public async Task DeleteAnime(Guid animeId)
    {
        await _adminService.DeleteAnime(animeId);
    }
}