using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken);
    
    Task<Category?> FindCategoryByIdAsync(Guid id, CancellationToken cancellationToken);
}