using System.ComponentModel.DataAnnotations;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KvolikDubBackend.Controllers;

[Route("api/anime")]
public class AnimeController : ControllerBase
{
    private readonly IAnimeService _animeService;

    public AnimeController(IAnimeService animeService)
    {
        _animeService = animeService;
    }
    
    /// <summary>
    /// Получить информацию о конкретном аниме
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    public async Task<AnimeDetailsDto> GetAnimeDetails(Guid id)
    {
        return await _animeService.GetAnimeDetails(id);
    }

    /// <summary>
    /// Получить список аниме для главной страницы
    /// </summary>
    [HttpGet]
    public async Task<List<AnimeListElementDto>> GetAnimeList()
    {
        return await _animeService.GetAnimeList();
    }

    [HttpGet]
    [Route("random")]
    public async Task<AnimeDetailsDto> GetRandomAnimeDetails()
    {
        return await _animeService.GetRandomAnimeDetails();
    }
}