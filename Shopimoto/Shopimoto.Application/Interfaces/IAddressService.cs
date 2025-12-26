using Shopimoto.Domain.Entities;

namespace Shopimoto.Application.Interfaces;

public interface IAddressService
{
    Task<List<Address>> GetUserAddressesAsync(Guid userId);
    Task<Address?> GetAddressByIdAsync(Guid id);
    Task<Address> AddAddressAsync(Address address);
    Task UpdateAddressAsync(Address address);
    Task DeleteAddressAsync(Guid id);
    Task SetDefaultAddressAsync(Guid userId, Guid addressId);
}
