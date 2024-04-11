using Domain.Common;

namespace Domain.Entities;

public sealed class Image : BaseAuditableEntity
{
    public required string BucketName { get; init; }
    public required string FileName { get; init; }
    public required string FileType { get; init; }
    
    /*** Relations ***/
    public required Advert Advert { get; init; }
    public Guid AdvertId { get; init; }
}