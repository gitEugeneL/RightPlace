using Domain.Common;

namespace Domain.Entities;

public sealed class RefreshToken : BaseAuditableEntity
{
    public required string Token { get; init; }
    public DateTime Expires { get; init; }
    
    /*** Relations ***/
    public required User User { get; init; }
    public Guid UserId { get; init; }
}