using Domain.Common;

namespace Domain.Entities;

public class Image : BaseAuditableEntity
{
    public required string BucketName { get; set; }
    public required string FileName { get; set; }
    public required string FileType { get; set; }
    
    // relations ----------------------------------------------------------
    public required Advert Advert { get; set; }
    public Guid AdvertId { get; set; }
}