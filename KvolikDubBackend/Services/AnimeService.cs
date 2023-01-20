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

    public async Task<AnimeDetailsDto> GetAnimeDetails(String shortName)
    {
        AnimeEntity animeEntity = await _context
            .Animes
            .Where(anime => anime.ShortName == shortName)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Cant find anime with shortName '{shortName}'");

        var animeDetailsDto = _mapping.Map<AnimeDetailsDto>(animeEntity);

        return animeDetailsDto;
    }

    public async Task<List<AnimeListElementDto>> GetVoicedAnimeList(String? search, IQueryCollection query)
    {
        CheckQueryAnimeList(query);
        search = search?.ToLower();
        var animeEntities = await _context
            .Animes
            .Where(anime => anime.VoiceoverStatus == VoiceoverStatus.Voiced)
            .ToListAsync();

        if (search != null)
        {
            animeEntities = animeEntities
                .Where(anime => anime.Name.ToLower().Contains(search) || anime.NameEng.ToLower().Contains(search))
                .ToList();
        }
        
        List<AnimeListElementDto> animeDtos = new List<AnimeListElementDto>();

        foreach (var animeEntity in animeEntities)
        {
            AnimeListElementDto animeListElementDto = _mapping.Map<AnimeListElementDto>(animeEntity);
            animeDtos.Add(animeListElementDto);
        }

        return animeDtos;
    }

    public async Task<List<AnimeListElementDto>> GetNotVoicedAnimeList(string? search, IQueryCollection query)
    {
        CheckQueryAnimeList(query);
        search = search?.ToLower();
        var animeEntities = await _context
            .Animes
            .Where(anime => anime.VoiceoverStatus == VoiceoverStatus.NotVoiced)
            .ToListAsync();

        if (search != null)
        {
            animeEntities = animeEntities
                .Where(anime => anime.Name.ToLower().Contains(search) || anime.NameEng.ToLower().Contains(search))
                .ToList();
        }
        
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
        return await GetAnimeDetails(animeEntities[randomAnimeIndex].ShortName);
    }

    private void CheckQueryAnimeList(IQueryCollection query)
    {
        foreach (var param in query)
        {
            if (param.Key != "search")
            {
                throw new BadRequestException($"Can't identify query param with key '{param.Key}'");
            }
        }
    }
}