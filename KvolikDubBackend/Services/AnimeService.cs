using AutoMapper;
using KvolikDubBackend.Models;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Models.Entities;
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
            .FirstOrDefaultAsync();

        var animeDetailsDto = _mapping.Map<AnimeDetailsDto>(animeEntity);

        return animeDetailsDto;
    }
}