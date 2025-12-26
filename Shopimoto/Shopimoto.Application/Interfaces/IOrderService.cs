using Shopimoto.Domain.Entities;

namespace Shopimoto.Application.Interfaces;

public interface IOrderService
{
    Task<Guid> CheckoutAsync(Guid userId, Guid addressId);
    Task<IEnumerable<Order>> GetMyOrdersAsync(Guid userId);
    Task<List<Order>> GetSellerOrdersAsync(Guid sellerId);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    Task CancelOrderAsync(Guid orderId, Guid userId);
    Task<(int TotalOrders, decimal TotalRevenue)> GetPlatformStatsAsync();
}
