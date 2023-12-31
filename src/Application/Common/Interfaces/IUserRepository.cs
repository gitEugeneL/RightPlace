using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExistByEmailAsync(string email, CancellationToken cancellationToken);

    Task<(IEnumerable<User> List, int Count)> GetUsersWithPaginationAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken);
    
    Task UpdateUserAsync(User user, CancellationToken cancellationToken);

    Task DeleteUserAsync(User user, CancellationToken cancellationToken);
    
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);

    Task<User?> FindUserByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<User?> FindUserByEmailAsync(string email, CancellationToken cancellationToken);
    
    Task<User?> FindUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}