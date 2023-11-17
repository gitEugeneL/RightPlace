namespace API.Entities;

public sealed class Role
{
    public Guid Id { get; set; }
    public required string Value { get; set; }
    
    // relation
    public List<User> Users { get; set; } = new();
}