using Shopimoto.Domain.Entities;

namespace Shopimoto.Application.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(string email, string password, string firstName, string lastName, string username, int? age, UserRole role);
    Task<User?> LoginAsync(string email, string password);
    Task UpdateProfileAsync(User user);
}
