using Shopimoto.Application.Interfaces;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;

namespace Shopimoto.Application.Services;

public class WishlistService : IWishlistService
{
    private readonly IWishlistRepository _wishlistRepository;

    public WishlistService(IWishlistRepository wishlistRepository)
    {
        _wishlistRepository = wishlistRepository;
    }

    public async Task<List<Wishlist>> GetUserWishlistAsync(Guid userId)
    {
        return (await _wishlistRepository.GetWishlistByUserIdAsync(userId)).ToList();
    }

    public async Task AddToWishlistAsync(Guid userId, Guid productId)
    {
        var existing = await _wishlistRepository.GetWishlistItemAsync(userId, productId);
        if (existing == null)
        {
            var wishlist = new Wishlist { UserId = userId, ProductId = productId };
            await _wishlistRepository.AddToWishlistAsync(wishlist);
        }
    }

    public async Task RemoveFromWishlistAsync(Guid userId, Guid productId)
    {
        var item = await _wishlistRepository.GetWishlistItemAsync(userId, productId);
        if (item != null)
        {
            await _wishlistRepository.RemoveFromWishlistAsync(item);
        }
    }

    public async Task<bool> IsInWishlistAsync(Guid userId, Guid productId)
    {
        var item = await _wishlistRepository.GetWishlistItemAsync(userId, productId);
        return item != null;
    }
}
