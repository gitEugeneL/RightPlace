using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Category : BaseEntity
{
    public required CategoryName Name { get; set; }
    public string? Description { get; set; }
    
    // relations ---------------------------------------------------
    public List<Advertisement> Advertisements { get; set; } = new();
}