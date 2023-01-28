using KvolikDubBackend.Models.Dtos;

namespace KvolikDubBackend.Services.Interfaces;

public interface IReviewService
{
    Task CreateReview(ReviewDto reviewDto, Guid animeId, String username);
    Task DeleteReview(Guid reviewId, String username);
    Task EditReview(ReviewDto editReviewDto, Guid reviewId, String username);
    Task SetRating(int grade, Guid animeId, String username);
    Task LikeReview(Guid reviewId, String username);
}