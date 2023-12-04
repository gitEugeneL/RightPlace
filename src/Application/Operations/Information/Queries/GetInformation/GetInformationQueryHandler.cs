using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MapsterMapper;
using MediatR;
using Info = Domain.Entities.Information;

namespace Application.Operations.Information.Queries.GetInformation;

public class GetInformationQueryHandler : IRequestHandler<GetInformationQuery, InformationResponse>
{
    private readonly IMapper _mapper;
    private readonly IInformationRepository _informationRepository;

    public GetInformationQueryHandler(IMapper mapper, IInformationRepository informationRepository)
    {
        _mapper = mapper;
        _informationRepository = informationRepository;
    }
    
    public async Task<InformationResponse> Handle(GetInformationQuery request, CancellationToken cancellationToken)
    {
        var information = await _informationRepository.FindInformationByIdAsync(request.Id, cancellationToken)
                          ?? throw new NotFoundException(nameof(Info), request.Id);

        return _mapper.Map<InformationResponse>(information);
    }
}
