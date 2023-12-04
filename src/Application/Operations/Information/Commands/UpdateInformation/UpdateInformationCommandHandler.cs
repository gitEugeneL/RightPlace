using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MapsterMapper;
using MediatR;
using Info = Domain.Entities.Information;

namespace Application.Operations.Information.Commands.UpdateInformation;

public class UpdateInformationCommandHandler : IRequestHandler<UpdateInformationCommand, InformationResponse>
{
    private readonly IMapper _mapper;
    private readonly IInformationRepository _informationRepository;

    public UpdateInformationCommandHandler(IMapper mapper, IInformationRepository informationRepository)
    {
        _mapper = mapper;
        _informationRepository = informationRepository;
    }
    
    public async Task<InformationResponse> Handle(UpdateInformationCommand request, CancellationToken cancellationToken)
    {
        var information = await _informationRepository
                              .FindInformationByIdAsync(request.InformationId, cancellationToken)
                          ?? throw new NotFoundException(nameof(Info), request.InformationId);
        
        if (information.Advert.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Info), request.InformationId);

        information.RoomCount = request.RoomCount ?? information.RoomCount;
        information.YearOfConstruction = request.YearOfConstruction ?? information.YearOfConstruction;
        information.EnergyEfficiencyRating = request.EnergyEfficiencyRating ?? information.EnergyEfficiencyRating;
        information.Elevator = request.Elevator ?? information.Elevator;
        information.Balcony = request.Balcony ?? information.Balcony;
        information.Floor = request.Floor ?? information.Floor;

        await _informationRepository.UpdateInformationAsync(information, cancellationToken);
        return _mapper.Map<InformationResponse>(information);
    }
}
