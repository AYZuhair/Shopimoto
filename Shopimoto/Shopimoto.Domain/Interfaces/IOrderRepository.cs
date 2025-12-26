using Shopimoto.Domain.Entities;

namespace Shopimoto.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
    Task<List<Order>> GetOrdersBySellerIdAsync(Guid sellerId);
    Task<Order?> GetByIdAsync(Guid id);
    Task UpdateAsync(Order order);
    Task<List<Order>> GetAllAsync();
}
