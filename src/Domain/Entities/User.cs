using Domain.Common;

namespace Domain.Entities;

public sealed class User : BaseAuditableEntity
{
    public required string Email { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    
    // relations ---------------------------------------------------
    public required Role Role { get; set; }
    public Guid RoleId { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; } = new();
}