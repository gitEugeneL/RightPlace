using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleByValueAsync(string value); 
}