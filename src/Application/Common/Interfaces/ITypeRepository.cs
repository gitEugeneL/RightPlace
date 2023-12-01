using Type = Domain.Entities.Type;

namespace Application.Common.Interfaces;

public interface ITypeRepository
{
    Task<IEnumerable<Type>> GetAllTypesAsync(CancellationToken cancellationToken);
    
    Task<Type?> FindTypeByIdAsync(Guid id, CancellationToken cancellationToken);
}