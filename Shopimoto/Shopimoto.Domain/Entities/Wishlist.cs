namespace Shopimoto.Domain.Entities;

public class Wishlist
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
