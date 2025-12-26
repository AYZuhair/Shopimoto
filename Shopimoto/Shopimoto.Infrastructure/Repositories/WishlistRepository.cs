using Microsoft.EntityFrameworkCore;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;
using Shopimoto.Infrastructure.Data;

namespace Shopimoto.Infrastructure.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private readonly ShopimotoDbContext _context;

    public WishlistRepository(ShopimotoDbContext context)
    {
        _context = context;
    }

    public async Task AddToWishlistAsync(Wishlist wishlist)
    {
        _context.Wishlists.Add(wishlist);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromWishlistAsync(Wishlist wishlist)
    {
        _context.Wishlists.Remove(wishlist);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Wishlist>> GetWishlistByUserIdAsync(Guid userId)
    {
        return await _context.Wishlists
            .Include(w => w.Product)
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<Wishlist?> GetWishlistItemAsync(Guid userId, Guid productId)
    {
        return await _context.Wishlists
            .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
    }
}
