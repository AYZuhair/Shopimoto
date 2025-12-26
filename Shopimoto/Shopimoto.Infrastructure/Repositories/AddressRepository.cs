using Microsoft.EntityFrameworkCore;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;
using Shopimoto.Infrastructure.Data;

namespace Shopimoto.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly ShopimotoDbContext _context;

    public AddressRepository(ShopimotoDbContext context)
    {
        _context = context;
    }

    public async Task<Address> AddAddressAsync(Address address)
    {
        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();
        return address;
    }

    public async Task<IEnumerable<Address>> GetAddressesByUserIdAsync(Guid userId)
    {
        return await _context.Addresses
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task<Address?> GetAddressByIdAsync(Guid id)
    {
        return await _context.Addresses.FindAsync(id);
    }

    public async Task UpdateAddressAsync(Address address)
    {
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAddressAsync(Address address)
    {
        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Address?> GetDefaultAddressAsync(Guid userId)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);
    }
}
