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
            .Include(anime => anime.Reviews)
            .Include(anime => anime.Ratings)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Cant find anime with shortName '{shortName}'");
        var animeDetailsDto = _mapping.Map<AnimeDetailsDto>(animeEntity);
        if (animeDetailsDto.averageRating == 0)
        {
            animeDetailsDto.averageRating = null;
        }
        
        for (int i = 0; i < animeEntity.Reviews.Count; i++)
        {
            var reviewEntity = await _context
                .Reviews
                .Where(rev => rev.Id == animeEntity.Reviews[i].Id)
                .Include(rev => rev.User)
                .FirstOrDefaultAsync();
            
            animeDetailsDto.reviews[i].name = reviewEntity.User.Name;
            animeDetailsDto.reviews[i].email = reviewEntity.User.Email;
            animeDetailsDto.reviews[i].avatarImageUrl = reviewEntity.User.AvatarImageUrl;
        }
        
        animeDetailsDto.reviews = animeDetailsDto
            .reviews
            .OrderByDescending(rev => rev.likes)
            .ThenBy(rev => rev.publishTime)
            .ToList();

        return animeDetailsDto;
    }

    public async Task<List<AnimeListElementDto>> GetVoicedAnimeList(String? search, IQueryCollection query)
    {
        Sorting? sort = null;
        
        CheckQueryAnimeList(query, ref sort);
        var animeEntities = await _context
            .Animes
            .Where(anime => anime.VoiceoverStatus == VoiceoverStatus.Озвучено)
            .ToListAsync();
        if (sort != null)
        {
            SortAnimes(ref animeEntities, sort);
        }
        
        if (search != null)
        {
            search = search?.ToLower();
            animeEntities = animeEntities
                .Where(anime => anime.Name.ToLower().Contains(search) || anime.NameEng.ToLower().Contains(search))
                .ToList();
        }
        
        List<AnimeListElementDto> animeDtos = new List<AnimeListElementDto>();

        foreach (var animeEntity in animeEntities)
        {
            AnimeListElementDto animeListElementDto = _mapping.Map<AnimeListElementDto>(animeEntity);
            if (animeListElementDto.averageRating == 0)
            {
                animeListElementDto.averageRating = null;
            }
            
            animeDtos.Add(animeListElementDto);
        }

        return animeDtos;
    }

    public async Task<List<AnimeListElementDto>> GetNotVoicedAnimeList(string? search, IQueryCollection query)
    {
        Sorting? sort = null;
        
        CheckQueryAnimeList(query, ref sort);
        var animeEntities = await _context
            .Animes
            .Where(anime => anime.VoiceoverStatus == VoiceoverStatus.Неозвучено)
            .ToListAsync();

        if (search != null)
        {
            search = search?.ToLower();
            animeEntities = animeEntities
                .Where(anime => anime.Name.ToLower().Contains(search) || anime.NameEng.ToLower().Contains(search))
                .ToList();
        }
        if (sort != null)
        {
            SortAnimes(ref animeEntities, sort);
        }
        
        List<AnimeListElementDto> animeDtos = new List<AnimeListElementDto>();

        foreach (var animeEntity in animeEntities)
        {
            AnimeListElementDto animeListElementDto = _mapping.Map<AnimeListElementDto>(animeEntity);
            if (animeListElementDto.averageRating == 0)
            {
                animeListElementDto.averageRating = null;
            }
            
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

    public async Task<List<string>> GetAllShortNames()
    {
        var animes = await _context
            .Animes
            .ToListAsync();
        List<String> shortNames = new();
        
        foreach (var anime in animes)
        {
            shortNames.Add(anime.ShortName);
        }
        
        return shortNames;
    }

    public async Task<MainPagePreviewDto> GetMainPagePreview()
    {
        var previewEntity = await _context
            .Previews
            .FirstOrDefaultAsync();
        var mainPagePreviewDto = _mapping.Map<MainPagePreviewDto>(previewEntity);

        return mainPagePreviewDto;
    }

    private void CheckQueryAnimeList(IQueryCollection query, ref Sorting? sort)
    {
        foreach (var param in query)
        {
            if (param.Key == "sort")
            {
                if (!Enum.IsDefined(typeof(Sorting), param.Value.ToString()))
                {
                    throw new BadRequestException("Incorrect sort value");
                }
                sort = (Sorting)Enum.Parse(typeof(Sorting), param.Value);
            }
        }
    }

    private void SortAnimes(ref List<AnimeEntity> animes, Sorting? sort)
    {
        switch (sort)
        {
            case Sorting.DateAsc:
                animes = animes.OrderBy(anime => anime.ReleaseFrom).ToList();
                break;
            case Sorting.DateDesc:
                animes = animes.OrderByDescending(anime => anime.ReleaseFrom).ToList();
                break;
            case Sorting.RatingAsc:
                var animesWithRating = animes
                    .OrderBy(anime => anime.AverageRating)
                    .Where(anime => anime.AverageRating != 0)
                    .ToList();
                var animesWithoutRating = animes
                    .Where(anime => anime.AverageRating == 0)
                    .ToList();
                animesWithRating.AddRange(animesWithoutRating);
                animes = animesWithRating;
                break;
            case Sorting.RatingDesc:
                animes = animes.OrderByDescending(anime => anime.AverageRating).ToList();
                break;
        }
    }
}