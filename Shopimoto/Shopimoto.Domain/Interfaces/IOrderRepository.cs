using Shopimoto.Domain.Entities;

namespace Shopimoto.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
    Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(Guid sellerId);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
}
