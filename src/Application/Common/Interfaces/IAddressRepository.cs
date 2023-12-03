using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAddressRepository
{
    Task UpdateAddressAsync(Address address, CancellationToken cancellationToken);
    
    Task<Address> CreateAddressAsync(Address address, CancellationToken cancellationToken);

    Task<Address?> FindAddressByIdAsync(Guid id, CancellationToken cancellationToken);
}