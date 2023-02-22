using System.ComponentModel.DataAnnotations;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Models.Enums;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
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
    [Route("{shortName}")]
    public async Task<AnimeDetailsDto> GetAnimeDetails(String shortName)
    {
        return await _animeService.GetAnimeDetails(shortName);
    }

    /// <summary>
    /// Получить список озвученных аниме (если в URL нет search, то выводятся все аниме)
    /// </summary>
    [HttpGet]
    public async Task<List<AnimeListElementDto>> GetVoicedAnimeList(String? search, Sorting sort)
    {
        return await _animeService.GetVoicedAnimeList(search, HttpContext.Request.Query);
    }
    
    /// <summary>
    /// Получить список неозвученных аниме (если в URL нет search, то выводятся все аниме)
    /// </summary>
    [HttpGet]
    [Route("soon")]
    public async Task<List<AnimeListElementDto>> GetNotVoicedAnimeList(String? search)
    {
        return await _animeService.GetNotVoicedAnimeList(search, HttpContext.Request.Query);
    }

    /// <summary>
    /// Поулчить детали случайного аниме
    /// </summary>
    [HttpGet]
    [Route("random")]
    public async Task<AnimeDetailsDto> GetRandomAnimeDetails()
    {
        return await _animeService.GetRandomAnimeDetails();
    }
    
    /// <summary>
    /// Получить все shorName
    /// </summary>
    [HttpGet]
    [Route("names")]
    public async Task<List<String>> GetAllShortNames()
    {
        return await _animeService.GetAllShortNames();
    }
    
    /// <summary>
    /// Получить првью для главной страницы
    /// </summary>
    [HttpGet]
    [Route("preview")]
    public async Task<MainPagePreviewDto> GetPreview()
    {
        return await _animeService.GetMainPagePreview();
    }
}