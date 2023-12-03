using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Addresses.Commands.CreateAddress;

public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, AddressResponse>
{
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;
    private readonly IAddressRepository _addressRepository;

    public CreateAddressCommandHandler(
        IMapper mapper, IAdvertRepository advertRepository, IAddressRepository addressRepository)
    {
        _mapper = mapper;
        _advertRepository = advertRepository;
        _addressRepository = addressRepository;
    }
    
    public async Task<AddressResponse> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var advert = await _advertRepository.FindAdvertByIdAsync(request.AdvertId, cancellationToken)
                     ?? throw new NotFoundException(nameof(Advert), request.AdvertId);
        
        if (advert.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Advert), request.AdvertId);

        if (advert.AddressId is not null)
            throw new AlreadyExistException(nameof(Address), advert.AddressId);
        
        var address = await _addressRepository.CreateAddressAsync(
            new Address
            {
                Advert = advert,
                City = request.City,
                Street = request.Street,
                Province = request.Province,
                House = request.House,
                GpsPosition = request.GpsPosition
            },
            cancellationToken
        );

        return _mapper.Map<AddressResponse>(address);
    }
}
