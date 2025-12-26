using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;
using BCrypt.Net;

namespace Shopimoto.Infrastructure.Data;

public class DatabaseSeeder
{
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;

    public DatabaseSeeder(IUserRepository userRepository, ICategoryRepository categoryRepository)
    {
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task SeedAsync()
    {
        // Seed Admin
        var adminEmail = "admin@shopimoto.com";
        var exists = await _userRepository.ExistsAsync(adminEmail);
        
        if (!exists)
        {
            var adminUser = new User
            {
                Email = adminEmail,
                FirstName = "Super",
                LastName = "Admin",
                Username = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = UserRole.Admin,
                MemberId = "ADMIN-001",
                CreatedAt = DateTime.UtcNow
            };
            
            await _userRepository.AddAsync(adminUser);
        }

        // Seed Categories
        var categories = await _categoryRepository.GetAllAsync();
        if (!categories.Any())
        {
            var defaultCategories = new List<Category>
            {
                new Category { Name = "Electronics", Description = "Gadgets and devices", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Clothing", Description = "Apparel for everyone", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Home", Description = "Kitchen and Furniture", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Books", Description = "Books and Literature", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Beauty", Description = "Cosmetics and Personal Care", CreatedAt = DateTime.UtcNow }
            };

            foreach (var cat in defaultCategories)
            {
                await _categoryRepository.AddAsync(cat);
            }
        }
    }
}
