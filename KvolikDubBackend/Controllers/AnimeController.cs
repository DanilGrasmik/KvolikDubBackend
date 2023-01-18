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

    [HttpGet]
    [Route("{id}")]
    public async Task<AnimeDetailsDto> GetAnimeDetails(Guid id)
    {
        return await _animeService.GetAnimeDetails(id);
    }

    [HttpGet]
    public async Task<List<AnimeListElementDto>> GetAnimeList()
    {
        return await _animeService.GetAnimeList();
    }
}