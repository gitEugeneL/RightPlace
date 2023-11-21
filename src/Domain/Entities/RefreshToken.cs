using Domain.Common;

namespace Domain.Entities;

public sealed class RefreshToken : BaseAuditableEntity
{
    public required string Token { get; set; }
    public DateTime Expires { get; set; }
    
    // relations ----------------------------------------------
    public required User User { get; set; }
    public Guid UserId { get; set; }
}