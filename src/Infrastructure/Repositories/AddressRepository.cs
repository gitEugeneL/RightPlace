using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly ApplicationDbContext _dataContext;

    public AddressRepository(ApplicationDbContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<Address> CreateAddressAsync(Address address, CancellationToken cancellationToken)
    {
        await _dataContext.Addresses.AddAsync(address, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
        return address;
    }

    public async Task UpdateAddressAsync(Address address, CancellationToken cancellationToken)
    {
        _dataContext.Addresses
            .Update(address);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Address?> FindAddressByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dataContext.Addresses
            .Include(address => address.Advert)
            .FirstOrDefaultAsync(address => address.Id == id, cancellationToken);
    }
}
