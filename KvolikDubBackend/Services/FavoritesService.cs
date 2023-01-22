using AutoMapper;
using KvolikDubBackend.Exceptions;
using KvolikDubBackend.Models;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KvolikDubBackend.Services;

public class FavoritesService : IFavoritesService
{
    private AppDbContext _context;
    private IMapper _mapper;

    public FavoritesService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task AddAnimeToFavorites(Guid animeId, String username)
    {
        var userEntity = await _context
            .Users
            .Include(user => user.FavoriteAnimes)
            .Where(user => user.Username == username)
            .FirstOrDefaultAsync();
        var animeEntity = await _context
            .Animes
            .Where(anime => anime.Id == animeId)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Cant find anime with id '{animeId}'");
        
        userEntity.FavoriteAnimes.Add(animeEntity);

        await _context.SaveChangesAsync();
    }

    public async Task<List<AnimeListElementDto>> GetFavoritesAnimes(string username)
    {
        var userEntity = await _context
            .Users
            .Where(user => user.Username == username)
            .Include(user => user.FavoriteAnimes)
            .FirstOrDefaultAsync();
        
        List<AnimeListElementDto> favoritesAnimeDtos = new();
        foreach (var anime in userEntity.FavoriteAnimes)
        {
            var animeDto = _mapper.Map<AnimeListElementDto>(anime);
            favoritesAnimeDtos.Add(animeDto);
        }

        return favoritesAnimeDtos;
    }
}