using Domain.Common;

namespace Domain.Entities;

public sealed class Role : BaseEntity
{
    public required string Value { get; set; }
    
    // relations ---------------------------------------------------
    public List<User> Users { get; set; } = new();
}