using KvolikDubBackend.Models.Dtos;

namespace KvolikDubBackend.Services.Interfaces;

public interface IFavoritesService
{
    Task AddAnimeToFavorites(Guid animeId, String username);
    Task<List<AnimeListElementDto>> GetFavoritesAnimes(String username);
    Task DeleteAnimeFromFavorites(Guid animeId, String username);
}