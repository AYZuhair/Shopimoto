using Microsoft.EntityFrameworkCore;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;
using Shopimoto.Infrastructure.Data;

namespace Shopimoto.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ShopimotoDbContext _context;

    public UserRepository(ShopimotoDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, Guid? excludeUserId = null)
    {
        var query = _context.Users.AsQueryable();
        
        if (excludeUserId.HasValue)
        {
            query = query.Where(u => u.Id != excludeUserId.Value);
        }
        
        return !await query.AnyAsync(u => u.Username == username);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }
}
