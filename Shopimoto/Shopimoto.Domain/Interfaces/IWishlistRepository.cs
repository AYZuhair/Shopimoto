using Shopimoto.Domain.Entities;

namespace Shopimoto.Domain.Interfaces;

public interface IWishlistRepository
{
    Task AddToWishlistAsync(Wishlist wishlist);
    Task RemoveFromWishlistAsync(Wishlist wishlist);
    Task<IEnumerable<Wishlist>> GetWishlistByUserIdAsync(Guid userId);
    Task<Wishlist?> GetWishlistItemAsync(Guid userId, Guid productId);
}
