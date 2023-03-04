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

        var imagePath = await UploadStaticFile(createAnimeDto.imageUri, "Animes");
        animeEntity.ImageUrl = imagePath;

        List<String> framesPaths = await UploadImages(createAnimeDto.frames, "Frames");
        animeEntity.Frames = framesPaths;

        if (createAnimeDto.previewVideoUri != null)
        {
            var prevVideoPath = await UploadStaticFile(createAnimeDto.previewVideoUri, "PreviewVideos");
            animeEntity.PreviewVideoUrl = prevVideoPath;
        }

        await _context.Animes.AddAsync(animeEntity);
        await _context.SaveChangesAsync();
    }

    public async Task EditAnime(CreateAnimeDto createAnimeDto, Guid animeId)
    {
        var animeEntity = await _context
            .Animes
            .Where(anime => anime.Id == animeId)
            .FirstOrDefaultAsync() ?? throw new NotFoundException("Cant find anime with Id '{animeId}'");
        
        if (createAnimeDto.frames.Count != 0)
        {
            List<string> newFrames = await UploadImages(createAnimeDto.frames, "Frames");
            foreach (var frame in animeEntity.Frames)
            {
                if (File.Exists("wwwroot/" + frame))
                {
                    File.Delete("wwwroot/" + frame);
                }
            }

            animeEntity.Frames = newFrames;
        }

        if (createAnimeDto.previewVideoUri != null)
        {
            var newPreview = await UploadStaticFile(createAnimeDto.previewVideoUri, "PreviewVideos");
            if (File.Exists("wwwroot/" + animeEntity.PreviewVideoUrl))
            {
                File.Delete("wwwroot/" + animeEntity.PreviewVideoUrl);
            }

            animeEntity.PreviewVideoUrl = newPreview;
        }
        
        if (createAnimeDto.imageUri != null)
        {
            var newImage = await UploadStaticFile(createAnimeDto.imageUri,"Animes");
            if (File.Exists("wwwroot/" + animeEntity.ImageUrl))
            {
                File.Delete("wwwroot/" + animeEntity.ImageUrl);
            }

            animeEntity.ImageUrl = newImage;
        }

        animeEntity.Description = createAnimeDto.description;
        animeEntity.Duration = createAnimeDto.duration;
        animeEntity.Genres = createAnimeDto.genres;
        animeEntity.Name = createAnimeDto.name;
        animeEntity.Type = createAnimeDto.type;
        animeEntity.AgeLimit = createAnimeDto.ageLimit;
        animeEntity.EpisodesAmount = createAnimeDto.episodesAmount;
        animeEntity.ExitStatus = createAnimeDto.exitStatus;
        animeEntity.NameEng = createAnimeDto.nameEng;
        animeEntity.PrimarySource = createAnimeDto.primarySource;
        animeEntity.ReleaseBy = createAnimeDto.releaseBy;
        animeEntity.ReleaseFrom = createAnimeDto.releaseFrom;
        animeEntity.ShortName = createAnimeDto.shortName;
        animeEntity.TrailerUrl = createAnimeDto.trailerUrl;
        animeEntity.IsMonophonic = createAnimeDto.isMonophonic;
        animeEntity.VoiceoverStatus = createAnimeDto.voiceoverStatus;
        animeEntity.PlayerLink = createAnimeDto.playerLink;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAnime(Guid animeId)
    {
        var animeEntity = await _context
            .Animes
            .Where(anime => anime.Id == animeId)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Cant find anime with Id '{animeId}'");
        
        DeleteAnimeStaticFiles(animeEntity);
        
        _context.Remove(animeEntity);
        await _context.SaveChangesAsync();
    }

    public async Task CreateAvatar(List<IFormFile> avatars)
    {
        var paths = await UploadImages(avatars, "Avatars");

        /*foreach (var path in paths)
        {
            AvatarEntity avatarEntity = new AvatarEntity
            {
                Id = new Guid(),
                ImageIrl = path
            };

            await _context.Avatars.AddAsync(avatarEntity);
            await _context.SaveChangesAsync();
        }*/
    }

    public async Task ChangePreview(string shortName)
    {
        var previewEntity = await _context
            .Previews
            .FirstOrDefaultAsync();
        var animeEntity = await _context
            .Animes
            .Where(anime => anime.ShortName == shortName)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"cant find anime with shortName {shortName}");
        
        previewEntity.ShortName = shortName;
        previewEntity.Description = animeEntity.Description;
        previewEntity.Name = animeEntity.Name;
        previewEntity.Type = animeEntity.Type;
        previewEntity.AgeLimit = animeEntity.AgeLimit;
        previewEntity.ReleaseFrom = animeEntity.ReleaseFrom;
        previewEntity.PreviewVideoUrl = animeEntity.PreviewVideoUrl;

        await _context.SaveChangesAsync();
    }


    private async Task<string> UploadStaticFile(IFormFile file, string directory)
    {
        var curDir = Directory.GetCurrentDirectory();
        var filePath = Path.Combine($"{curDir}/wwwroot/{directory}", file.FileName);
        if (File.Exists(filePath))
        {
            throw new BadRequestException($"File with name {file.FileName} already exists");
        }
        using (FileStream ms = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(ms);
        }
        return filePath.Substring(8);
    }
    
    private async Task<List<string>> UploadImages(List<IFormFile> files, string dirName)
    {
        var curDir = Directory.GetCurrentDirectory();
        List<string> filePaths = new();

        foreach (var file in files)
        {
            var filePath = Path.Combine($"{curDir}/wwwroot/{dirName}", file.FileName);
            if (File.Exists(filePath))
            {
                throw new BadRequestException($"File with name {file.FileName} already exists");
            }
            filePaths.Add(filePath.Substring(8));
        }
        
        foreach (var file in files)
        {
            var filePath = Path.Combine($"{curDir}/wwwroot/{dirName}", file.FileName);
            using (FileStream ms = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(ms);
            }
        }

        return filePaths;
    }

    private void DeleteAnimeStaticFiles(AnimeEntity animeEntity)
    {
        if (File.Exists("wwwroot/" + animeEntity.PreviewVideoUrl))
        {
            File.Delete("wwwroot/" + animeEntity.PreviewVideoUrl);
        }

        if (File.Exists("wwwroot/" + animeEntity.ImageUrl))
        {
            File.Delete("wwwroot/" + animeEntity.ImageUrl);
        }
        foreach (var frame in animeEntity.Frames)
        {
            if (File.Exists("wwwroot/" + frame))
            {
                File.Delete("wwwroot/" + frame);
            }
        }
    }
}