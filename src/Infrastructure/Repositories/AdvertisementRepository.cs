using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AdvertisementRepository : IAdvertisementRepository
{
    private readonly ApplicationDbContext _dataContext;
    
    public AdvertisementRepository(ApplicationDbContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> AdvertisementExistByTitleAsync(string title, CancellationToken cancellationToken)
    {
        return await _dataContext.Advertisements
            .AnyAsync(a => a.Title.ToLower() == title.ToLower(), cancellationToken);
    }

    public async Task UpdateAdvertisementAsync(Advertisement advertisement, CancellationToken cancellationToken)
    {
        _dataContext.Advertisements
            .Update(advertisement);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAdvertisementAsync(Advertisement advertisement, CancellationToken cancellationToken)
    {
        _dataContext.Advertisements.Remove(advertisement);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Advertisement> CreateAdvertisementAsync(Advertisement advertisement,
        CancellationToken cancellationToken)
    {
        await _dataContext.Advertisements
            .AddAsync(advertisement, cancellationToken);
        await _dataContext
            .SaveChangesAsync(cancellationToken);
        return advertisement;
    }

    public async Task<Advertisement?> FindAdvertisementByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dataContext.Advertisements
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
    
    public async Task<(IEnumerable<Advertisement> List, int Count)> GetAdvertisementWithPaginationAsync(
            CancellationToken cancellationToken,
            int pageNumber, 
            int pageSize, 
            bool sortOrderAsc = true,
            string? sortBy = null,
            string? city = null,
            Guid? categoryId = null,
            Guid? typeId = null,
            Guid? ownerId = null
        )
    {
        var query = _dataContext.Advertisements
            .Include(a => a.Category)
            .Include(a => a.Type)
            .Include(a => a.User)
            .Include(a => a.Address)
            .AsQueryable();
        
        query = categoryId.HasValue 
            ? query.Where(a => a.CategoryId == categoryId) 
            : query;
        
        query = typeId.HasValue
            ? query.Where(a => a.TypeId == typeId)
            : query;
        
        query = ownerId.HasValue
            ? query.Where(a => a.UserId == ownerId)
            : query;
        
        query = city is not null
            ? query.Where(a => a.Address != null && a.Address.City == city)
            : query;
        
        if (sortBy is not null)
        {
            switch (sortBy.ToLower())
            {
                case "created":
                    query = sortOrderAsc
                        ? query.OrderBy(a => a.Created)
                        : query.OrderByDescending(a => a.Created);
                    break;
                
                case "price":
                    query = sortOrderAsc
                        ? query.OrderBy(a => (double) a.Price)
                        : query.OrderByDescending(a => (double) a.Price);
                    break;
            }
        }
        
        var count = await query.CountAsync(cancellationToken);
        
        var advertisements = await query
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return (advertisements, count);
    }
}
