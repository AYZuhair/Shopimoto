using Shopimoto.Domain.Entities;

namespace Shopimoto.Domain.Interfaces;

public interface IAddressRepository
{
    Task<Address> AddAddressAsync(Address address);
    Task<IEnumerable<Address>> GetAddressesByUserIdAsync(Guid userId);
    Task<Address?> GetAddressByIdAsync(Guid id);
    Task UpdateAddressAsync(Address address);
    Task DeleteAddressAsync(Address address);
    Task<Address?> GetDefaultAddressAsync(Guid userId);
}
