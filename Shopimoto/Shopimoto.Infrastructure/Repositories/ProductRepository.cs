using Microsoft.EntityFrameworkCore;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;
using Shopimoto.Infrastructure.Data;

namespace Shopimoto.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShopimotoDbContext _context;

    public ProductRepository(ShopimotoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.Include(p => p.Seller).ToListAsync();
    }

    public async Task<List<Product>> GetBySellerIdAsync(Guid sellerId)
    {
        return await _context.Products.Where(p => p.SellerId == sellerId).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.Include(p => p.Seller).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Shopimoto.Domain.Common.PagedResult<Product>> SearchAsync(Shopimoto.Domain.DTOs.ProductSearchParams searchParams)
    {
        var query = _context.Products
            .Include(p => p.Seller)
            .AsQueryable();

        // Filter by Search Term
        if (!string.IsNullOrWhiteSpace(searchParams.SearchTerm))
        {
            var term = searchParams.SearchTerm.ToLower();
            query = query.Where(p => p.Title.ToLower().Contains(term) || 
                                     p.Description.ToLower().Contains(term));
        }

        // Filter by Category
        if (!string.IsNullOrWhiteSpace(searchParams.Category))
        {
             query = query.Where(p => p.Category == searchParams.Category);
        }

        // Sorting
        query = searchParams.SortBy switch
        {
            "PriceAsc" => query.OrderBy(p => p.Price),
            "PriceDesc" => query.OrderByDescending(p => p.Price),
            "Newest" => query.OrderByDescending(p => p.CreatedAt),
            "TopRated" => query.OrderByDescending(p => p.Reviews.Average(r => r.Rating)), // This might fail if no reviews, need handling? EF Core translates Average well usually, but let's be careful.
            _ => query.OrderBy(p => p.Title) // Default
        };

        // Pagination
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((searchParams.PageNumber - 1) * searchParams.PageSize)
            .Take(searchParams.PageSize)
            .ToListAsync();

        return new Shopimoto.Domain.Common.PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = searchParams.PageNumber,
            PageSize = searchParams.PageSize
        };
    }
}
