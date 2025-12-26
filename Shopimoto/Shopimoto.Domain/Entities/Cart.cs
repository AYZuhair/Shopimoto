using System.ComponentModel.DataAnnotations.Schema;

namespace Shopimoto.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; }
    
    public Guid? BuyerId { get; set; } // Nullable for guest carts if needed
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public List<CartItem> Items { get; set; } = new();
}

public class CartItem
{
    public Guid Id { get; set; }
    
    public Guid CartId { get; set; }
    public Cart? Cart { get; set; }
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public int Quantity { get; set; }
}
