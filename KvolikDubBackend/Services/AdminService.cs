using AutoMapper;
using KvolikDubBackend.Exceptions;
using KvolikDubBackend.Models;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Models.Entities;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KvolikDubBackend.Services;

public class AdminService : IAdminService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AdminService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task CreateAnime(CreateAnimeDto createAnimeDto)
    {
        var animeEntity = _mapper.Map<AnimeEntity>(createAnimeDto);
        
        await _context.AddAsync(animeEntity);
        await _context.SaveChangesAsync();
    }

    public async Task EditAnime(CreateAnimeDto createAnimeDto, Guid animeId)
    {
        var animeEntity = await _context
            .Animes
            .Where(anime => anime.Id == animeId)
            .FirstOrDefaultAsync() ?? throw new NotFoundException("Cant find anime with Id '{animeId}'");

        animeEntity.Description = createAnimeDto.description;
        animeEntity.Duration = createAnimeDto.duration;
        animeEntity.Frames = createAnimeDto.frames;
        animeEntity.Genres = createAnimeDto.genres;
        animeEntity.Name = createAnimeDto.name;
        animeEntity.Type = createAnimeDto.type;
        animeEntity.AgeLimit = createAnimeDto.ageLimit;
        animeEntity.EpisodesAmount = createAnimeDto.episodesAmount;
        animeEntity.ExitStatus = createAnimeDto.exitStatus;
        animeEntity.ImageUrl = createAnimeDto.imageUrl;
        animeEntity.NameEng = createAnimeDto.nameEng;
        animeEntity.PrimarySource = createAnimeDto.primarySource;
        animeEntity.ReleaseBy = createAnimeDto.releaseBy;
        animeEntity.ReleaseFrom = createAnimeDto.releaseFrom;
        animeEntity.ShortName = createAnimeDto.shortName;
        animeEntity.TrailerUrl = createAnimeDto.trailerUrl;
        animeEntity.VoiceoverStatus = createAnimeDto.voiceoverStatus;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAnime(Guid animeId)
    {
        var animeEntity = await _context
            .Animes
            .Where(anime => anime.Id == animeId)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Cant find anime with Id '{animeId}'");

        _context.Remove(animeEntity);
        await _context.SaveChangesAsync();
    }
}