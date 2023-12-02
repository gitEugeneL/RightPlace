using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Advertisements.Queries.GetAdvertisement;

public class GetAdvertisementQueryHandler : IRequestHandler<GetAdvertisementQuery, AdvertisementResponse>
{
    private readonly IMapper _mapper;
    private readonly IAdvertisementRepository _advertisementRepository;
    
    public GetAdvertisementQueryHandler(IMapper mapper, IAdvertisementRepository advertisementRepository)
    {
        _mapper = mapper;
        _advertisementRepository = advertisementRepository;
    }
    
    public async Task<AdvertisementResponse> Handle(GetAdvertisementQuery request, CancellationToken cancellationToken)
    {
        var advertisement = await _advertisementRepository.FindAdvertisementByIdAsync(request.Id, cancellationToken)
                            ?? throw new NotFoundException(nameof(Advertisement), request.Id);

        return _mapper.Map<AdvertisementResponse>(advertisement);
    }
}
