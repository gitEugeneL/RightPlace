using Domain.Common;

namespace Domain.Entities;

public class Advertisement : BaseAuditableEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    
    // relations ---------------------------------------------------
    public required User User { get; set; }
    public Guid UserId { get; set; }

    public required Category Category { get; set; }
    public Guid CategoryId { get; set; }

    public required Type Type { get; set; }
    public Guid TypeId { get; set; }

    public Address? Address { get; set; }
    public Guid? AddressId { get; set; }

    public Information? Information { get; set; }
    public Guid? InformationId { get; set; } 
}