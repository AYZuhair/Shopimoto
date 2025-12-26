using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;
using BCrypt.Net;

namespace Shopimoto.Infrastructure.Data;

public class DatabaseSeeder
{
    private readonly IUserRepository _userRepository;

    public DatabaseSeeder(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task SeedAsync()
    {
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
    }
}
