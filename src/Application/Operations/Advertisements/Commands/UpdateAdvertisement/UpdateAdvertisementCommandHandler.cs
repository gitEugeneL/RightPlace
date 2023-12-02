using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Advertisements.Commands.UpdateAdvertisement;

public class UpdateAdvertisementCommandHandler : IRequestHandler<UpdateAdvertisementCommand, AdvertisementResponse>
{
    private readonly IMapper _mapper;
    private readonly IAdvertisementRepository _advertisementRepository;

    public UpdateAdvertisementCommandHandler(IMapper mapper, IAdvertisementRepository advertisementRepository)
    {
        _mapper = mapper;
        _advertisementRepository = advertisementRepository;
    }

    public async Task<AdvertisementResponse> Handle(UpdateAdvertisementCommand request, CancellationToken cancellationToken)
    {
        var advertisement =
            await _advertisementRepository.FindAdvertisementByIdAsync(request.AdvertisementId, cancellationToken)
            ?? throw new NotFoundException(nameof(Advertisement), request.AdvertisementId);

        if (advertisement.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Advertisement), request.AdvertisementId);

        advertisement.Description = request.Description ?? advertisement.Description;
        advertisement.Price = request.Price ?? advertisement.Price;

        await _advertisementRepository.UpdateAdvertisementAsync(advertisement, cancellationToken);
        return _mapper.Map<AdvertisementResponse>(advertisement);
    }
}