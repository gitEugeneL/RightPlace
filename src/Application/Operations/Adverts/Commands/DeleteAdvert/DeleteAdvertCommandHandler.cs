using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Adverts.Commands.DeleteAdvert;

public class DeleteAdvertCommandHandler : IRequestHandler<DeleteAdvertCommand, Unit>
{
    private readonly IAdvertRepository _advertRepository;

    public DeleteAdvertCommandHandler(IAdvertRepository advertRepository)
    {
        _advertRepository = advertRepository;
    }

    public async Task<Unit> Handle(DeleteAdvertCommand request, CancellationToken cancellationToken)
    {
        var advertisement =
            await _advertRepository.FindAdvertByIdAsync(request.AdvertId, cancellationToken)
            ?? throw new NotFoundException(nameof(Advert), request.AdvertId);

        if (advertisement.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Advert), request.AdvertId);

        await _advertRepository.DeleteAdvertAsync(advertisement, cancellationToken);
        return await Unit.Task;
    }
}