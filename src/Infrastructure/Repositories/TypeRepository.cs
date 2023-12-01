using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Type = Domain.Entities.Type;

namespace Infrastructure.Repositories;

public class TypeRepository : ITypeRepository
{
    private readonly ApplicationDbContext _dataContext;
    
    public TypeRepository(ApplicationDbContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<Type>> GetAllTypesAsync(CancellationToken cancellationToken)
    {
        return await _dataContext.Types
            .ToListAsync(cancellationToken);
    }

    public async Task<Type?> FindTypeByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dataContext.Types
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}