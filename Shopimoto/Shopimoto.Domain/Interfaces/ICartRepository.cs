using Shopimoto.Domain.Entities;

namespace Shopimoto.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByBuyerIdAsync(Guid buyerId);
        Task<Cart> CreateCartAsync(Guid buyerId);
        Task AddCartItemAsync(Guid cartId, Guid productId, int quantity);
        Task UpdateCartItemQuantityAsync(Guid cartItemId, int quantity);
        Task RemoveCartItemAsync(Guid cartItemId);
        Task ClearCartAsync(Guid cartId);
    }
}
