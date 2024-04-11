using Domain.Common;

namespace Domain.Entities;

public sealed class Address : BaseAuditableEntity
{
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string Province { get; set; }
    public required string House { get; set; }
    public string? GpsPosition { get; set; } 
    
    /*** Relations ***/
    public required Advert Advert { get; init; }
}