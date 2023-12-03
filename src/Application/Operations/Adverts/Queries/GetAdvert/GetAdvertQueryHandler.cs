using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Adverts.Queries.GetAdvert;

public class GetAdvertQueryHandler : IRequestHandler<GetAdvertQuery, AdvertsResponse>
{
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;
    
    public GetAdvertQueryHandler(IMapper mapper, IAdvertRepository advertRepository)
    {
        _mapper = mapper;
        _advertRepository = advertRepository;
    }
    
    public async Task<AdvertsResponse> Handle(GetAdvertQuery request, CancellationToken cancellationToken)
    {
        var advertisement = await _advertRepository.FindAdvertByIdAsync(request.Id, cancellationToken)
                            ?? throw new NotFoundException(nameof(Advert), request.Id);

        return _mapper.Map<AdvertsResponse>(advertisement);
    }
}
