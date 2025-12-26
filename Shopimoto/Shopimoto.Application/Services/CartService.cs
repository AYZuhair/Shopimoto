using Shopimoto.Application.Interfaces;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;

namespace Shopimoto.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<Cart> GetOrCreateCartAsync(Guid buyerId)
        {
            var cart = await _cartRepository.GetCartByBuyerIdAsync(buyerId);
            if (cart == null)
            {
                cart = await _cartRepository.CreateCartAsync(buyerId);
            }
            return cart;
        }

        public async Task AddToCartAsync(Guid buyerId, Guid productId, int quantity)
        {
            // Optional: Check stock here using _productRepository if needed
            // For now, we trust the add
            var cart = await GetOrCreateCartAsync(buyerId);
            await _cartRepository.AddCartItemAsync(cart.Id, productId, quantity);
        }

        public async Task UpdateQuantityAsync(Guid cartItemId, int quantity)
        {
            await _cartRepository.UpdateCartItemQuantityAsync(cartItemId, quantity);
        }

        public async Task RemoveItemAsync(Guid cartItemId)
        {
            await _cartRepository.RemoveCartItemAsync(cartItemId);
        }

        public async Task ClearCartAsync(Guid buyerId)
        {
            var cart = await _cartRepository.GetCartByBuyerIdAsync(buyerId);
            if (cart != null)
            {
                await _cartRepository.ClearCartAsync(cart.Id);
            }
        }
    }
}
