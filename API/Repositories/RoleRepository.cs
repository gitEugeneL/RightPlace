using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetRoleByValueAsync(string value);
}

public class RoleRepository : IRoleRepository
{
    private readonly DataContext _dataContext;
    public RoleRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<Role?> GetRoleByValueAsync(string value)
    {
        return await _dataContext.Roles.FirstOrDefaultAsync(r => r.Value == value);
    }
}