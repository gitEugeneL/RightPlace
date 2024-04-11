using Domain.Common;

namespace Domain.Entities;

public sealed class Category : BaseEntity
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    
    /*** Relations ***/
    public List<Advert> Adverts { get; init; } = new();
}