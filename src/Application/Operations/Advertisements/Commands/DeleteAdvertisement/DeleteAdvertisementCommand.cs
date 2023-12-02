using MediatR;

namespace Application.Operations.Advertisements.Commands.DeleteAdvertisement;

public record DeleteAdvertisementCommand : IRequest<Unit>
{
    public Guid CurrentUserId { get; private set; }
    public required Guid AdvertisementId { get; init; }

    public DeleteAdvertisementCommand SetCurrentUserId(string id)
    { 
        CurrentUserId = Guid.Parse(id);
        return this;
    }
}

