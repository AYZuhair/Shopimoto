using Shopimoto.Domain.Entities;

namespace Shopimoto.Application.Interfaces;

public interface IReviewService
{
    Task<List<Review>> GetProductReviewsAsync(Guid productId);
    Task<List<Review>> GetUserReviewsAsync(Guid userId);
    Task<Review> AddReviewAsync(Review review);
    Task DeleteReviewAsync(Guid id);
}
