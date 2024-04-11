using Domain.Common;

namespace Domain.Entities;

public sealed class Information : BaseAuditableEntity
{
    public required uint RoomCount { get; set; }
    public required uint Area { get; init; }
    public required short YearOfConstruction { get; set; }
    public bool Elevator { get; set; }
    public bool Balcony { get; set; }
    public uint? Floor { get; set; }
    public string? EnergyEfficiencyRating { get; set; } 
    
    /*** Relations ***/
    public required Advert Advert { get; init; }
}