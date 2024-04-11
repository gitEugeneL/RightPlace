using Domain.Common;

namespace Domain.Entities;

public sealed class Advert : BaseAuditableEntity
{
    public required string Title { get; init; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    
    /*** Relations ***/
    public List<Image> Images { get; init; } = new();
    
    public required User User { get; init; }
    public Guid UserId { get; init; }

    public required Category Category { get; init; }
    public Guid CategoryId { get; init; }

    public required Type Type { get; init; }
    public Guid TypeId { get; init; }
    
    public Address? Address { get; init; }
    public Guid? AddressId { get; init; }

    public Information? Information { get; init; }
    public Guid? InformationId { get; init; } 
}