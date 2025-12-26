using Shopimoto.Application.Interfaces;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;

namespace Shopimoto.Application.Services;

public class OrderService : IOrderService
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IProductRepository _productRepository; // Injected

    public OrderService(ICartRepository cartRepository, IOrderRepository orderRepository, IAddressRepository addressRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _addressRepository = addressRepository;
        _productRepository = productRepository;
    }

    public async Task<Guid> CheckoutAsync(Guid userId, Guid addressId)
    {
        var cart = await _cartRepository.GetCartByBuyerIdAsync(userId);
        if (cart == null || !cart.Items.Any())
        {
            throw new Exception("Cart is empty");
        }
        
        var address = await _addressRepository.GetAddressByIdAsync(addressId);
        if (address == null)
        {
            throw new Exception("Shipping address not found");
        }

        var order = new Order
        {
            BuyerId = userId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Processing, // Assume payment is "mocked" as successful immediately
            Items = new List<OrderItem>(),
            // Snapshot Address
            ShippingStreet = address.Street,
            ShippingCity = address.City,
            ShippingState = address.State,
            ShippingZipCode = address.ZipCode,
            ShippingCountry = address.Country
        };

        decimal totalAmount = 0;

        foreach (var item in cart.Items)
        {
            // Verify stock
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null) throw new Exception($"Product not found: {item.ProductId}");
            
            if (product.Stock < item.Quantity)
            {
                throw new Exception($"Insufficient stock for product '{product.Title}'. Available: {product.Stock}, Requested: {item.Quantity}");
            }

            // Deduct Stock
            product.Stock -= item.Quantity;
            await _productRepository.UpdateAsync(product);

            var orderItem = new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price // Use current price from DB to be safe
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

    public async Task<List<Order>> GetSellerOrdersAsync(Guid sellerId)
    {
        return await _orderRepository.GetOrdersBySellerIdAsync(sellerId);
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        return await _orderRepository.GetByIdAsync(orderId);
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            await _orderRepository.UpdateAsync(order);
        }
    }

    public async Task CancelOrderAsync(Guid orderId, Guid userId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null) throw new Exception("Order not found");
        
        if (order.BuyerId != userId) throw new Exception("Unauthorized to cancel this order");

        if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Processing)
        {
            throw new Exception("Cannot cancel order that has already been shipped or delivered.");
        }

        order.Status = OrderStatus.Cancelled;
        await _orderRepository.UpdateAsync(order);

        // Restore Stock
        foreach (var item in order.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.Stock += item.Quantity;
                await _productRepository.UpdateAsync(product);
            }
        }
    }

    public async Task<(int TotalOrders, decimal TotalRevenue)> GetPlatformStatsAsync()
    {
        var allOrders = await _orderRepository.GetAllAsync();
        return (allOrders.Count, allOrders.Sum(o => o.TotalAmount));
    }
}
