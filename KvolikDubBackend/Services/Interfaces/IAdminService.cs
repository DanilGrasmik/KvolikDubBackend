using KvolikDubBackend.Models.Dtos;

namespace KvolikDubBackend.Services.Interfaces;

public interface IAdminService
{
    Task CreateAnime(CreateAnimeDto createAnimeDto);
    Task EditAnime(CreateAnimeDto createAnimeDto, Guid animeId);
    Task DeleteAnime(Guid animeId);
    Task CreateAvatar(CreateAvatarDto createAvatarDto);
    Task ChangePreview(string shortName);
}