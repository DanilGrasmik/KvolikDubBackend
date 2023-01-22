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

    [HttpPost]
    [Authorize]
    [Route("{id}")]
    public async Task AddAnimeToFavorites(Guid id)
    {
        await _favoritesService.AddAnimeToFavorites(id, User.Identity.Name);
    }

    [HttpGet]
    [Authorize]
    public async Task<List<AnimeListElementDto>> GetFavorites()
    {
        return await _favoritesService.GetFavoritesAnimes(User.Identity.Name);
    }
}