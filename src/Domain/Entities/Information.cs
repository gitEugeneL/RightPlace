using Domain.Common;

namespace Domain.Entities;

public class Information : BaseAuditableEntity
{
    public required uint RoomCount { get; set; }
    public required uint Area { get; set; }
    public required short YearOfConstruction { get; set; }
    public uint? Floor { get; set; }
    public string? EnergyEfficiencyRating { get; set; } 
    public bool Elevator { get; set; } = false;
    public bool Balcony { get; set; } = false;
    
    // relations ---------------------------------------------
    public required Advertisement Advertisement { get; set; }
}