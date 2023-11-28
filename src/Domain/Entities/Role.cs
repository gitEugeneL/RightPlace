using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public sealed class Role : BaseEntity
{
    public required RoleName Value { get; set; }
    
    // relations ---------------------------------------------------
    public List<User> Users { get; set; } = new();
}