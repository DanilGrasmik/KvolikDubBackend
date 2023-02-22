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
    public async Task CreateNewAnime([FromForm] CreateAnimeDto createAnimeDto)
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
    public async Task EditNewAnime([FromForm] CreateAnimeDto createAnimeDto, Guid animeId)
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
    
    /// <summary>
    /// Добавить картинку для рандомной авы пользователей
    /// </summary>
    [HttpPost]
    [Route("avatar")]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Authorize(Policy = "AdminRequest")]
    public async Task CreateAvatar(List<IFormFile> avatars)
    {
        await _adminService.CreateAvatar(avatars);
    }
    
    /// <summary>
    /// Изменить превью на главной странице
    /// </summary>
    [HttpPut]
    [Route("preview/{shortName}")]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Authorize(Policy = "AdminRequest")]
    public async Task ChangePreview(string shortName)
    {
        await _adminService.ChangePreview(shortName);
    }
}