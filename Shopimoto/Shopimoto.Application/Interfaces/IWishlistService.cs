using Shopimoto.Domain.Entities;

namespace Shopimoto.Application.Interfaces;

public interface IWishlistService
{
    Task<List<Wishlist>> GetUserWishlistAsync(Guid userId);
    Task AddToWishlistAsync(Guid userId, Guid productId);
    Task RemoveFromWishlistAsync(Guid userId, Guid productId);
    Task<bool> IsInWishlistAsync(Guid userId, Guid productId);
}
