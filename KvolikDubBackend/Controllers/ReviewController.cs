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

    /// <summary>
    /// Создать комментарий
    /// </summary>
    [HttpPost]
    [Authorize]
    [Route("review")]
    public async Task CreateReview([FromBody] ReviewDto reviewDto, Guid animeId)
    {
        await _reviewService.CreateReview(reviewDto, animeId, User.Identity.Name);
    }
    
    /// <summary>
    /// Удалить комментарий
    /// </summary>
    [HttpDelete]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Route("review/{reviewId}")]
    public async Task DeleteReview(Guid reviewId)
    {
        await _reviewService.DeleteReview(reviewId, User.Identity.Name);
    }
    
    /*/// <summary>
    /// Изменить комментарий
    /// </summary>
    [HttpPut]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Route("review/{reviewId}")]
    public async Task EditReview([FromBody] ReviewDto editReviewDto, Guid reviewId)
    {
        await _reviewService.EditReview(editReviewDto, reviewId, User.Identity.Name);
    }*/

    /// <summary>
    /// Поставить оценку аниме
    /// </summary>
    [HttpPost]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Route("rating")]
    public async Task SetRating(int grade, Guid animeId)
    {
        await _reviewService.SetRating(grade, animeId, User.Identity.Name);
    }
    
    /// <summary>
    /// Лайкнуть комментарий пользователя
    /// </summary>
    [HttpPost]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Route("review/{reviewId}/like")]
    public async Task LikeReview(Guid reviewId)
    {
        await _reviewService.LikeReview(reviewId, User.Identity.Name);
    }
    
    /// <summary>
    /// Убрать лайк с комментария пользователя 
    /// </summary>
    [HttpDelete]
    [Authorize]
    [Authorize(Policy = "TokenValidation")]
    [Route("review/{reviewId}/like")]
    public async Task RemoveLike(Guid reviewId)
    {
        await _reviewService.RemoveLike(reviewId, User.Identity.Name);
    }
}