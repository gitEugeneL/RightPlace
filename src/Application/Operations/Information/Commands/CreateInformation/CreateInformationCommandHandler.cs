using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;
using Info = Domain.Entities.Information;

namespace Application.Operations.Information.Commands.CreateInformation;

public class CreateInformationCommandHandler : IRequestHandler<CreateInformationCommand, InformationResponse>
{
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;
    private readonly IInformationRepository _informationRepository;

    public CreateInformationCommandHandler(
        IMapper mapper, IAdvertRepository advertRepository, IInformationRepository informationRepository)
    {
        _mapper = mapper;
        _advertRepository = advertRepository;
        _informationRepository = informationRepository;
    }
    
    public async Task<InformationResponse> Handle(CreateInformationCommand request, CancellationToken cancellationToken)
    {
        var advert = await _advertRepository.FindAdvertByIdAsync(request.AdvertId, cancellationToken)
                     ?? throw new NotFoundException(nameof(Advert), request.AdvertId);

        if (advert.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Advert), request.AdvertId);

        if (advert.InformationId is not null)
            throw new AlreadyExistException(nameof(Information), advert.InformationId);

        
        var information = await _informationRepository.CreateInformationAsync(
            new Info
            {
                Advert = advert,
                RoomCount = request.RoomCount,
                Area = request.Area,
                YearOfConstruction = request.YearOfConstruction,
                Floor = request.Floor,
                EnergyEfficiencyRating = request.EnergyEfficiencyRating,
                Elevator = request.Elevator ?? false,
                Balcony = request.Balcony ?? false
            },
            cancellationToken
        );

        return _mapper.Map<InformationResponse>(information);
    }
}
