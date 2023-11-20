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

    public async Task<User> CreateUserAsync(User user)
    {
        await _dataContext.Users.AddAsync(user);
        await _dataContext.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        return await _dataContext.Users
            .Include(u => u.Role)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User?> FindUserByRefreshTokenAsync(string refreshToken)
    {
        return await _dataContext.Users
            .Include(u => u.RefreshTokens)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));
    }

    public async Task<bool> UserExistAsync(string email)
    {
        return await _dataContext.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
}