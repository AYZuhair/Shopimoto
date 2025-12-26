using Shopimoto.Application.Interfaces;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;
using BCrypt.Net;

namespace Shopimoto.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> RegisterAsync(string email, string password, string firstName, string lastName, string username, int? age, UserRole role)
    {
        if (await _userRepository.ExistsAsync(email))
        {
            throw new Exception("User with this email already exists.");
        }

        if (!string.IsNullOrEmpty(username) && !await _userRepository.IsUsernameUniqueAsync(username))
        {
             throw new Exception("Username is already taken.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            Username = username,
            Age = age,
            Role = role,
            CreatedAt = DateTime.UtcNow,
            MemberId = GenerateMemberId()
        };

        return await _userRepository.AddAsync(user);
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }

    public async Task UpdateProfileAsync(User user)
    {
        if (!string.IsNullOrEmpty(user.Username) && !await _userRepository.IsUsernameUniqueAsync(user.Username, user.Id))
        {
             throw new Exception("Username is already taken.");
        }
        
        await _userRepository.UpdateAsync(user);
    }

    private string GenerateMemberId()
    {
        // Simple generation: SHP-{Random 6 digits}
        // In production, check for uniqueness
        return $"SHP-{Random.Shared.Next(100000, 999999)}";
    }
}
