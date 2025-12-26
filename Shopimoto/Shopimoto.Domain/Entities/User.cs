using System.ComponentModel.DataAnnotations;

namespace Shopimoto.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    public int? Age { get; set; }

    [MaxLength(500)]
    public string ProfilePictureUrl { get; set; } = string.Empty;

    [MaxLength(20)]
    public string MemberId { get; set; } = string.Empty; // e.g. "SHP-123456"
    
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum UserRole
{
    Buyer,
    Seller,
    Admin
}
