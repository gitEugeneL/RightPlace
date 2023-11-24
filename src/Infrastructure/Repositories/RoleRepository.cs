using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _dataContext;
    public RoleRepository(ApplicationDbContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<Role?> GetRoleByValueAsync(string value, CancellationToken cancellationToken)
    {
        return await _dataContext.Roles.FirstOrDefaultAsync(r => r.Value == value, cancellationToken);
    }
}