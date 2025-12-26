using Shopimoto.Application.Interfaces;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;

namespace Shopimoto.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<List<Review>> GetProductReviewsAsync(Guid productId)
    {
        return (await _reviewRepository.GetReviewsByProductIdAsync(productId)).ToList();
    }

    public async Task<List<Review>> GetUserReviewsAsync(Guid userId)
    {
        return (await _reviewRepository.GetReviewsByUserIdAsync(userId)).ToList();
    }

    public async Task<Review> AddReviewAsync(Review review)
    {
        return await _reviewRepository.AddReviewAsync(review);
    }

    public async Task DeleteReviewAsync(Guid id)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(id);
        if (review != null)
        {
            await _reviewRepository.DeleteReviewAsync(review);
        }
    }
}
