using Shopimoto.Domain.Entities;

namespace Shopimoto.Application.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetOrCreateCartAsync(Guid buyerId);
        Task AddToCartAsync(Guid buyerId, Guid productId, int quantity);
        Task UpdateQuantityAsync(Guid cartItemId, int quantity);
        Task RemoveItemAsync(Guid cartItemId);
        Task ClearCartAsync(Guid buyerId);
    }
}
