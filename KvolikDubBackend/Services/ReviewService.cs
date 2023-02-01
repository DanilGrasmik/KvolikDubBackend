using AutoMapper;
using KvolikDubBackend.Exceptions;
using KvolikDubBackend.Models;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Models.Entities;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KvolikDubBackend.Services;

public class ReviewService : IReviewService
{
    private AppDbContext _context;
    private IMapper _mapper;

    public ReviewService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task CreateReview(ReviewDto reviewDto, Guid animeId, String username)
    {
        var reviewText = reviewDto.reviewText.ToLower().Replace('ё','е');
        await CheckForBadWords(reviewText);
        var animeEntity = await _context
            .Animes
            .Where(anime => anime.Id == animeId)
            .Include(anime => anime.Reviews)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Cant find anime with Id '{animeId}'");
        var userEntity = await _context
            .Users
            .Where(user => user.Username == username)
           // .Include(user => user.Reviews)
            .FirstOrDefaultAsync();
        
        var reviewEntity = new ReviewEntity()
        {
            Id = new Guid(),
            Anime = animeEntity,
            User = userEntity,
            PublishTime = DateTime.UtcNow,
            ReviewText = reviewDto.reviewText,
            Likes = 0
        };
        //userEntity.Reviews.Add(reviewEntity);
        animeEntity.Reviews.Add(reviewEntity);

        await _context.Reviews.AddAsync(reviewEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReview(Guid reviewId, string username)
    {
        var userEntity = await _context
            .Users
            .Where(user => user.Username == username)
            .FirstOrDefaultAsync();
        var reviewEntity = await _context
            .Reviews
            .Where(rev => rev.Id == reviewId)
            .Include(rev => rev.User)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Cant find review with Id '{reviewId}'");
        if (reviewEntity.User.Username != username && !userEntity.IsAdmin)
        {
            throw new ForbiddenException("You cant delete not your own review");
        }

        _context.Reviews.Remove(reviewEntity);
        await _context.SaveChangesAsync();
    }

    public async Task EditReview(ReviewDto editReviewDto, Guid reviewId, string username)
    {
        var reviewEntity = await _context
            .Reviews
            .Where(rev => rev.Id == reviewId)
            .Include(rev => rev.User)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Cant find review with Id '{reviewId}'");
        if (reviewEntity.User.Username != username)
        {
            throw new ForbiddenException("You cant delete not your own review");
        }
        reviewEntity.ReviewText = editReviewDto.reviewText;
        reviewEntity.PublishTime = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task SetRating(int grade, Guid animeId, string username)
    {
        if (grade < 1 || grade > 10)
        {
            throw new BadRequestException("Rating must be in range 1 to 10");
        }
        var rating = await _context
            .Ratings
            .Include(rat => rat.User)
            .Where(rat => rat.User.Username == username)
            .FirstOrDefaultAsync();
        if (rating != null)
        {
            rating.Grade = grade;
        }
        else
        {
            var userEntity = await _context
                .Users
                .Where(user => user.Username == username)
                .Include(user => user.Ratings)
                .FirstOrDefaultAsync();
            var animeEntity = await _context
                .Animes
                .Where(anime => anime.Id == animeId)
                .Include(anime => anime.Ratings)
                .FirstOrDefaultAsync() ?? throw new NotAcceptableException($"Cant find anime with Id '{animeId}'");
            
            RatingEntity ratingEntity = new RatingEntity()
            {
                Id = new Guid(),
                Grade = grade,
                User = userEntity,
                Anime = animeEntity
            };
        
            userEntity.Ratings.Add(ratingEntity);
            animeEntity.Ratings.Add(ratingEntity);
            animeEntity.AverageRating = GetAverageRating(animeEntity.Ratings);
            
            await _context.Ratings.AddAsync(ratingEntity);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task LikeReview(Guid revId, string username)
    {
        var userEntity = await _context
            .Users
            .Where(user => user.Username == username)
            .FirstOrDefaultAsync();
        var reviewEntity = await _context
            .Reviews
            .Where(rev => rev.Id == revId)
            .Include(rev => rev.LikedUsers)
            .FirstOrDefaultAsync() ?? throw new NotFoundException("Cant find review with Id '{revId}'");

        if (reviewEntity.LikedUsers.Contains(userEntity))
        {
            throw new ConflictException("You cant like review more then 1 time");
        }
        
        reviewEntity.LikedUsers.Add(userEntity);
        reviewEntity.Likes++;

        await _context.SaveChangesAsync();
    }

    private async Task CheckForBadWords(String text)
    {
        var badWords = await _context.BadWords.ToListAsync();
        foreach (var word in badWords)
        {
            if (text.Contains(word.Word))
            {
                throw new NotAcceptableException("Bad word in review");
            }
        }
    }

    private double GetAverageRating(List<RatingEntity> ratings)
    {
        double ratingSum = 0;
        foreach (var rate in ratings)
        {
            ratingSum += rate.Grade;
        }

        return ratingSum / ratings.Count;
    }
}