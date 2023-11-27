using Domain.Common;

namespace Domain.Entities;

public class Type : BaseEntity
{
    public required string Name { get; set; }
    
    // relations ---------------------------------------------------
    public List<Advertisement> Advertisements { get; set; } = new();
}