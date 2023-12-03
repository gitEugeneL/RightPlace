using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Adverts.Commands.UpdateAdvert;

public record UpdateAdvertCommand : IRequest<AdvertsResponse>
{
    public Guid CurrentUserId { get; private set; }

    [Required] 
    public required Guid AdvertId { get; init; }

    public string? Description { get; init; }

    [Range(10, double.MaxValue)]
    public decimal? Price { get; init; }

    public void SetCurrentUserId(string id) => CurrentUserId = Guid.Parse(id);
}