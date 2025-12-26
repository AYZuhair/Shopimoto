using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopimoto.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    
    public Guid BuyerId { get; set; }
    public User? Buyer { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    
    // Shipping Address Snapshot
    [MaxLength(200)]
    public string ShippingStreet { get; set; } = string.Empty;
    [MaxLength(100)]
    public string ShippingCity { get; set; } = string.Empty;
    [MaxLength(100)]
    public string ShippingState { get; set; } = string.Empty;
    [MaxLength(20)]
    public string ShippingZipCode { get; set; } = string.Empty;
    [MaxLength(100)]
    public string ShippingCountry { get; set; } = string.Empty;
    
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    
    public List<OrderItem> Items { get; set; } = new();
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
