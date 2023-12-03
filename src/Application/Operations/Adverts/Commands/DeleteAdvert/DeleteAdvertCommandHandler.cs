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
            await _advertRepository.FindAdvertisementByIdAsync(request.AdvertisementId, cancellationToken)
            ?? throw new NotFoundException(nameof(Advert), request.AdvertisementId);

        if (advertisement.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Advert), request.AdvertisementId);

        await _advertRepository.DeleteAdvertisementAsync(advertisement, cancellationToken);
        return await Unit.Task;
    }
}