using System.ComponentModel.DataAnnotations;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KvolikDubBackend.Controllers;

[Route("api/favorites")]
public class FavoritesController : ControllerBase
{
    private IFavoritesService _favoritesService;

    public FavoritesController(IFavoritesService favoritesService)
    {
        _favoritesService = favoritesService;
    }

    /// <summary>
    /// Добавить аниме в избранное
    /// </summary>
    [HttpPost]
    [Authorize]
    [Route("{id}")]
    public async Task AddAnimeToFavorites(Guid id)
    {
        await _favoritesService.AddAnimeToFavorites(id, User.Identity.Name);
    }

    /// <summary>
    /// Получить список избранных
    /// </summary>
    [HttpGet]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    public async Task<List<AnimeListElementDto>> GetFavorites()
    {
        return await _favoritesService.GetFavoritesAnimes(User.Identity.Name);
    }

    /// <summary>
    /// Удалить аниме из избранного  
    /// </summary>
    [HttpDelete]
    [Authorize]
    [Route("{id}")]
    public async Task DeleteAnime(Guid id)
    {
        await _favoritesService.DeleteAnimeFromFavorites(id, User.Identity.Name);
    }
}