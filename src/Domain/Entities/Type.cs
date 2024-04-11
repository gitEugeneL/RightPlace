using Domain.Common;

namespace Domain.Entities;

public sealed class Type : BaseEntity
{
    public required string Name { get; init; }
    
    /*** Relations ***/
    public List<Advert> Adverts { get; init; } = new();
}