using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, AddressResponse>
{
    private readonly IMapper _mapper;
    private readonly IAddressRepository _addressRepository;

    public UpdateAddressCommandHandler(IMapper mapper, IAddressRepository addressRepository)
    {
        _mapper = mapper;
        _addressRepository = addressRepository;
    }
    
    public async Task<AddressResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.FindAddressByIdAsync(request.AddressId, cancellationToken)
                      ?? throw new NotFoundException(nameof(Address), request.AddressId);
        
        if (address.Advert.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Address), request.AddressId);
        
        address.City = request.City ?? address.City;
        address.Street = request.Street ?? address.Street;
        address.Province = request.Province ?? address.Province;
        address.House = request.House ?? address.House;
        address.GpsPosition = request.GpsPosition ?? address.GpsPosition;

        await _addressRepository.UpdateAddressAsync(address, cancellationToken);
        return _mapper.Map<AddressResponse>(address);
    }
}
