using Shopimoto.Domain.Entities;

namespace Shopimoto.Application.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetProductsAsync();
    Task<List<Product>> GetSellerProductsAsync(Guid sellerId);
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product> CreateProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(Guid id);
}
