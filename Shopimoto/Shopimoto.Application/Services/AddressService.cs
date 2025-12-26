using Shopimoto.Application.Interfaces;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;

namespace Shopimoto.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<List<Address>> GetUserAddressesAsync(Guid userId)
    {
        return (await _addressRepository.GetAddressesByUserIdAsync(userId)).ToList();
    }

    public async Task<Address?> GetAddressByIdAsync(Guid id)
    {
        return await _addressRepository.GetAddressByIdAsync(id);
    }

    public async Task<Address> AddAddressAsync(Address address)
    {
        // If this is the first address, make it default
        var existing = await _addressRepository.GetAddressesByUserIdAsync(address.UserId);
        if (!existing.Any())
        {
            address.IsDefault = true;
        }
        else if (address.IsDefault)
        {
            // If new one is default, unset existing default
            var currentDefault = existing.FirstOrDefault(a => a.IsDefault);
            if (currentDefault != null)
            {
                currentDefault.IsDefault = false;
                await _addressRepository.UpdateAddressAsync(currentDefault);
            }
        }
        
        return await _addressRepository.AddAddressAsync(address);
    }

    public async Task UpdateAddressAsync(Address address)
    {
        if (address.IsDefault)
        {
             var existing = await _addressRepository.GetAddressesByUserIdAsync(address.UserId);
             var currentDefault = existing.FirstOrDefault(a => a.IsDefault && a.Id != address.Id);
             if (currentDefault != null)
             {
                 currentDefault.IsDefault = false;
                 await _addressRepository.UpdateAddressAsync(currentDefault);
             }
        }
        await _addressRepository.UpdateAddressAsync(address);
    }

    public async Task DeleteAddressAsync(Guid id)
    {
        var address = await _addressRepository.GetAddressByIdAsync(id);
        if (address != null)
        {
            await _addressRepository.DeleteAddressAsync(address);
        }
    }

    public async Task SetDefaultAddressAsync(Guid userId, Guid addressId)
    {
        var addresses = await _addressRepository.GetAddressesByUserIdAsync(userId);
        var newDefault = addresses.FirstOrDefault(a => a.Id == addressId);
        var oldDefault = addresses.FirstOrDefault(a => a.IsDefault);

        if (newDefault == null) return;

        if (oldDefault != null)
        {
            oldDefault.IsDefault = false;
            await _addressRepository.UpdateAddressAsync(oldDefault);
        }

        newDefault.IsDefault = true;
        await _addressRepository.UpdateAddressAsync(newDefault);
    }
}
