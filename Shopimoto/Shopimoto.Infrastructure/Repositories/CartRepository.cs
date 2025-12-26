using Microsoft.EntityFrameworkCore;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;
using Shopimoto.Infrastructure.Data;

namespace Shopimoto.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ShopimotoDbContext _context;

        public CartRepository(ShopimotoDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByBuyerIdAsync(Guid buyerId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        }

        public async Task<Cart> CreateCartAsync(Guid buyerId)
        {
            var cart = new Cart
            {
                BuyerId = buyerId,
                CreatedAt = DateTime.UtcNow,
                Items = new List<CartItem>()
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task AddCartItemAsync(Guid cartId, Guid productId, int quantity)
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null) return;

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                // If item exists, just increase quantity
                existingItem.Quantity += quantity;
            }
            else
            {
                // Add new item
                cart.Items.Add(new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemQuantityAsync(Guid cartItemId, int quantity)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    _context.CartItems.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveCartItemAsync(Guid cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(Guid cartId)
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.Items);
                await _context.SaveChangesAsync();
            }
        }
    }
}
