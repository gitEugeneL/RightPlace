using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);
    Task<bool> UserExistAsync(string email);
    Task<User?> FindUserByEmailAsync(string email);
    Task<User?> FindUserByRefreshTokenAsync(string refreshToken);
    Task UpdateUserAsync(User user);
}