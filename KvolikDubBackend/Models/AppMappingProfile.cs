using AutoMapper;
using KvolikDubBackend.Models;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Models.Entities;

namespace KvolikDubBackend.Models;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {			
        CreateMap<AnimeDetailsDto, AnimeEntity>().ReverseMap();
        CreateMap<AnimeEntity, AnimeListElementDto>();

    }
}