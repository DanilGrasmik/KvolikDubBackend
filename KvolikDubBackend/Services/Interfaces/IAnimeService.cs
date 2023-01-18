using KvolikDubBackend.Models.Dtos;

namespace KvolikDubBackend.Services.Interfaces;

public interface IAnimeService
{
    Task<AnimeDetailsDto> GetAnimeDetails(Guid id);
}