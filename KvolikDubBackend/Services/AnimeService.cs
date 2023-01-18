using AutoMapper;
using KvolikDubBackend.Exceptions;
using KvolikDubBackend.Models;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Models.Entities;
using KvolikDubBackend.Models.Enums;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KvolikDubBackend.Services;

public class AnimeService : IAnimeService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapping;

    public AnimeService(AppDbContext context, IMapper mapping)
    {
        _context = context;
        _mapping = mapping;
    }

    public async Task<AnimeDetailsDto> GetAnimeDetails(Guid id)
    {
        AnimeEntity animeEntity = await _context
            .Animes
            .Where(anime => anime.Id == id)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Cant find anime with id '{id}'");

        var animeDetailsDto = _mapping.Map<AnimeDetailsDto>(animeEntity);

        return animeDetailsDto;
    }

    public async Task<List<AnimeListElementDto>> GetAnimeList()
    {
        var animeEntities = await _context
            .Animes
            .Where(anime => anime.VoiceoverStatus == VoiceoverStatus.Voiced)
            .ToListAsync();
        List<AnimeListElementDto> animeDtos = new List<AnimeListElementDto>();

        foreach (var animeEntity in animeEntities)
        {
            AnimeListElementDto animeListElementDto = _mapping.Map<AnimeListElementDto>(animeEntity);
            animeDtos.Add(animeListElementDto);
        }

        return animeDtos;
    }

    public async Task<AnimeDetailsDto> GetRandomAnimeDetails()
    {
        var animeEntities = await _context
            .Animes
            .ToListAsync();
        Random random = new Random();
        int randomAnimeIndex = random.Next(0, animeEntities.Count);
        return await GetAnimeDetails(animeEntities[randomAnimeIndex].Id);
    }
}