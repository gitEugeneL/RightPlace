using Domain.Common;

namespace Domain.Entities;

public class Address : BaseAuditableEntity
{
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string Province { get; set; }
    public required string House { get; set; }
    public string? GpsPosition { get; set; } 
    
    // relations ---------------------------------------------------
    public required Advert Advert { get; set; }
}