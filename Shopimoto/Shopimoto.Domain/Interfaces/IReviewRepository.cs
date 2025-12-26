using Shopimoto.Domain.Entities;

namespace Shopimoto.Domain.Interfaces;

public interface IReviewRepository
{
    Task<Review> AddReviewAsync(Review review);
    Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId);
    Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId);
    Task<Review?> GetReviewByIdAsync(Guid id);
    Task DeleteReviewAsync(Review review);
}
