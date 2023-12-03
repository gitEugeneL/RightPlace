using MediatR;

namespace Application.Operations.Adverts.Commands.DeleteAdvert;

public record DeleteAdvertCommand : IRequest<Unit>
{
    public Guid CurrentUserId { get; private set; }
    public required Guid AdvertId { get; init; }

    public DeleteAdvertCommand SetCurrentUserId(string id)
    { 
        CurrentUserId = Guid.Parse(id);
        return this;
    }
}

