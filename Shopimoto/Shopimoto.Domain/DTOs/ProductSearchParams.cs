namespace Shopimoto.Domain.DTOs;

public class ProductSearchParams
{
    public string? SearchTerm { get; set; }
    public string? Category { get; set; }
    public string? SortBy { get; set; } // "PriceAsc", "PriceDesc", "Newest", "TopRated"
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
