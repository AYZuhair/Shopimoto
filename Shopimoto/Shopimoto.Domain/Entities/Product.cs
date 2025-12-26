using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopimoto.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    
    public int Stock { get; set; }
    
    public string ImageUrl { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
    
    // Foreign Key to Seller (User)
    public Guid SellerId { get; set; }
    public User? Seller { get; set; }
    
    public List<Review> Reviews { get; set; } = new();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
