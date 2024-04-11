using Domain.Common;

namespace Domain.Entities;

public sealed class User : BaseAuditableEntity
{
    public required string Email { get; init; }
    public required byte[] PasswordHash { get; init; }
    public required byte[] PasswordSalt { get; init; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    
    /*** Relations ***/
    public required Role Role { get; init; }
    public Guid RoleId { get; init; }

    public List<RefreshToken> RefreshTokens { get; init; } = new();

    public List<Advert> Adverts { get; init; } = new();
}