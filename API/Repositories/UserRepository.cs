using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);
    Task<bool> UserExistAsync(string email);
}

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;
    public UserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        await _dataContext.Users.AddAsync(user);
        await _dataContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UserExistAsync(string email)
    {
        return await _dataContext.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
}