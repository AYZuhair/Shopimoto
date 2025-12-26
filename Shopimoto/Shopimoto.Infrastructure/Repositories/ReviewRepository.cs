using Microsoft.EntityFrameworkCore;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;
using Shopimoto.Infrastructure.Data;

namespace Shopimoto.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ShopimotoDbContext _context;

    public ReviewRepository(ShopimotoDbContext context)
    {
        _context = context;
    }

    public async Task<Review> AddReviewAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId)
    {
        return await _context.Reviews
            .Include(r => r.Product)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review?> GetReviewByIdAsync(Guid id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task DeleteReviewAsync(Review review)
    {
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }
}
