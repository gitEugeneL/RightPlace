namespace API.Entities;

public sealed class User
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedDate { get; set; }

    // relations --------------------------------------------------------------------
    public required Role Role { get; set; }
    public Guid RoleId { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; } = new();
}