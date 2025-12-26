using System.ComponentModel.DataAnnotations;

namespace Shopimoto.Domain.Entities;

public class Review
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    [Range(1, 5)]
    public int Rating { get; set; }
    
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
