using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopimoto.Domain.Entities;

public class Address
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Street { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string State { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string ZipCode { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;
    
    public bool IsDefault { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
