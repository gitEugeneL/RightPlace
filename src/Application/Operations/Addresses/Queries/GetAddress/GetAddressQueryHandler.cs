using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Addresses.Queries.GetAddress;

public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, AddressResponse>
{
    private readonly IMapper _mapper;
    private readonly IAddressRepository _addressRepository;

    public GetAddressQueryHandler(IMapper mapper, IAddressRepository addressRepository)
    {
        _mapper = mapper;
        _addressRepository = addressRepository;
    }
    
    public async Task<AddressResponse> Handle(GetAddressQuery request, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.FindAddressByIdAsync(request.Id, cancellationToken)
                      ?? throw new NotFoundException(nameof(Address), request.Id);
        
        return _mapper.Map<AddressResponse>(address);
    }
}
