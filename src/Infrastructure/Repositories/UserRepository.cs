using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dataContext;
    public UserRepository(ApplicationDbContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        await _dataContext.Users.AddAsync(user, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> FindUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dataContext.Users
            .Include(u => u.Role)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
    }
    
    public async Task<User?> FindUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return await _dataContext.Users
            .Include(u => u.RefreshTokens)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.RefreshTokens
                .Any(rt => rt.Token == refreshToken), cancellationToken);
    }
    
    public async Task<bool> UserExistByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dataContext.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
    }
}