using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Adverts.Commands.UpdateAdvert;

public record UpdateAdvertCommand : IRequest<AdvertsResponse>
{
    public Guid CurrentUserId { get; private set; }

    [Required] 
    public required Guid AdvertisementId { get; set; }

    public string? Description { get; set; }

    [Range(10, double.MaxValue)]
    public decimal? Price { get; set; }

    public void SetCurrentUserId(string id) => CurrentUserId = Guid.Parse(id);
}