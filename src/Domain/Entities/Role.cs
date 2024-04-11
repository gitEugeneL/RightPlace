using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public sealed class Role : BaseEntity
{
    public required RoleName Value { get; init; }
    
    /*** Relations ***/
    public List<User> Users { get; init; } = new();
}