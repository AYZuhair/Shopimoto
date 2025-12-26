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
