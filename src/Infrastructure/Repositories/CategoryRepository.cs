using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dataContext;

    public CategoryRepository(ApplicationDbContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _dataContext.Categories
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Category?> FindCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dataContext.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}