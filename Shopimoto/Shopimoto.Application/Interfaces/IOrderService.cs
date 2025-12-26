using Shopimoto.Domain.Entities;

namespace Shopimoto.Application.Interfaces;

public interface IOrderService
{
    Task<Guid> CheckoutAsync(Guid userId);
    Task<IEnumerable<Order>> GetMyOrdersAsync(Guid userId);
    Task<IEnumerable<Order>> GetSellerOrdersAsync(Guid sellerId);
    Task<Order?> GetOrderDetailsAsync(Guid orderId);
}
