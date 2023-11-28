using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleByValueAsync(RoleName value, CancellationToken cancellationToken); 
}