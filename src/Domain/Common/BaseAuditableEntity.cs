namespace Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; } = DateTime.UtcNow;
    public DateTime? Updated { get; set; }
}