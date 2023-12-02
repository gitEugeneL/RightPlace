using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Advertisements.Commands.DeleteAdvertisement;

public class DeleteAdvertisementCommandHandler : IRequestHandler<DeleteAdvertisementCommand, Unit>
{
    private readonly IAdvertisementRepository _advertisementRepository;

    public DeleteAdvertisementCommandHandler(IAdvertisementRepository advertisementRepository)
    {
        _advertisementRepository = advertisementRepository;
    }

    public async Task<Unit> Handle(DeleteAdvertisementCommand request, CancellationToken cancellationToken)
    {
        var advertisement =
            await _advertisementRepository.FindAdvertisementByIdAsync(request.AdvertisementId, cancellationToken)
            ?? throw new NotFoundException(nameof(Advertisement), request.AdvertisementId);

        if (advertisement.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Advertisement), request.AdvertisementId);

        await _advertisementRepository.DeleteAdvertisementAsync(advertisement, cancellationToken);
        return await Unit.Task;
    }
}