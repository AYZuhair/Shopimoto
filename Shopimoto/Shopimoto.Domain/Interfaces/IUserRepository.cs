using Shopimoto.Domain.Entities;

namespace Shopimoto.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User> AddAsync(User user);
    Task<bool> ExistsAsync(string email);
    Task UpdateAsync(User user);
    Task<bool> IsUsernameUniqueAsync(string username, Guid? excludeUserId = null);
}
