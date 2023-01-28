using System.ComponentModel.DataAnnotations;
using KvolikDubBackend.Models.Dtos;
using KvolikDubBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KvolikDubBackend.Controllers;

[Route("api/anime/{animeId}")]
public class ReviewController : ControllerBase
{
    private IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost]
    [Authorize]
    [Route("review")]
    public async Task CreateReview([FromBody] ReviewDto reviewDto, Guid animeId)
    {
        await _reviewService.CreateReview(reviewDto, animeId, User.Identity.Name);
    }
    
    [HttpDelete]
    [Authorize]
    [Route("review/{reviewId}")]
    public async Task DeleteReview(Guid reviewId)
    {
        await _reviewService.DeleteReview(reviewId, User.Identity.Name);
    }
    
    [HttpPut]
    [Authorize]
    [Route("review/{reviewId}")]
    public async Task EditReview([FromBody] ReviewDto editReviewDto, Guid reviewId)
    {
        await _reviewService.EditReview(editReviewDto, reviewId, User.Identity.Name);
    }

    [HttpPost]
    [Authorize]
    [Route("rating")]
    public async Task SetRating(int grade, Guid animeId)
    {
        await _reviewService.SetRating(grade, animeId, User.Identity.Name);
    }
    
    [HttpPost]
    [Authorize]
    [Route("review/{reviewId}/like")]
    public async Task LikeReview(Guid reviewId)
    {
        await _reviewService.LikeReview(reviewId, User.Identity.Name);
    }
}