using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Models.Enums;

namespace KvolikDubBackend.Services.Interfaces;

public interface IAnimeService
{
    Task<AnimeDetailsDto> GetAnimeDetails(String shortName);
    Task<List<AnimeListElementDto>> GetVoicedAnimeList(String? search, IQueryCollection query);
    Task<List<AnimeListElementDto>> GetNotVoicedAnimeList(String? search, IQueryCollection query);
    Task<AnimeDetailsDto> GetRandomAnimeDetails();
    /*Task<List<AnimeListElementDto>> GetVoicedAnimesBySearch(String search);
    Task<List<AnimeListElementDto>> GetNotVoicedAnimesBySearch(String search);*/
}