using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class InformationRepository : IInformationRepository
{
    private readonly ApplicationDbContext _dataContext;

    public InformationRepository(ApplicationDbContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<Information> CreateInformationAsync(Information information, CancellationToken cancellationToken)
    {
        await _dataContext.Information.AddAsync(information, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
        return information;
    }
    
    public async Task UpdateInformationAsync(Information information, CancellationToken cancellationToken)
    {
        _dataContext.Information
            .Update(information);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Information?> FindInformationByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dataContext.Information
            .Include(information => information.Advert)
            .FirstOrDefaultAsync(information => information.Id == id, cancellationToken);
    }
}
