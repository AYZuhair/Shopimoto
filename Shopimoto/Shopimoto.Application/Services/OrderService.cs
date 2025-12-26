using Shopimoto.Application.Interfaces;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;

namespace Shopimoto.Application.Services;

public class OrderService : IOrderService
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    // In a real app, inject IProductRepository to decrement stock

    public OrderService(ICartRepository cartRepository, IOrderRepository orderRepository)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
    }

    public async Task<Guid> CheckoutAsync(Guid userId)
    {
        var cart = await _cartRepository.GetCartByBuyerIdAsync(userId);
        if (cart == null || !cart.Items.Any())
        {
            throw new Exception("Cart is empty");
        }

        var order = new Order
        {
            BuyerId = userId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Processing, // Assume payment is "mocked" as successful immediately
            Items = new List<OrderItem>()
        };

        decimal totalAmount = 0;

        foreach (var item in cart.Items)
        {
            var orderItem = new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.Product!.Price // Assuming Product is included in query
            };
            
            totalAmount += orderItem.UnitPrice * orderItem.Quantity;
            order.Items.Add(orderItem);
        }

        order.TotalAmount = totalAmount;

        await _orderRepository.CreateOrderAsync(order);
        await _cartRepository.ClearCartAsync(cart.Id);

        return order.Id;
    }

    public async Task<IEnumerable<Order>> GetMyOrdersAsync(Guid userId)
    {
        return await _orderRepository.GetOrdersByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Order>> GetSellerOrdersAsync(Guid sellerId)
    {
        return await _orderRepository.GetOrdersBySellerIdAsync(sellerId);
    }

    public async Task<Order?> GetOrderDetailsAsync(Guid orderId)
    {
        return await _orderRepository.GetOrderByIdAsync(orderId);
    }
}
