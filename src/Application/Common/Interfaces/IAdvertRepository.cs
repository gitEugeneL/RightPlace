using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAdvertRepository
{
    Task<bool> AdvertisementExistByTitleAsync(string title, CancellationToken cancellationToken);

    Task UpdateAdvertisementAsync(Advert advert, CancellationToken cancellationToken);

    Task DeleteAdvertisementAsync(Advert advert, CancellationToken cancellationToken);
    
    Task<Advert> CreateAdvertisementAsync(Advert advert, CancellationToken cancellationToken);

    Task<Advert?> FindAdvertisementByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<(IEnumerable<Advert> List, int Count)> GetAdvertisementWithPaginationAsync(
        CancellationToken cancellationToken,
        int pageNumber,
        int pageSize,
        bool sortOrderAsc = true,
        string? sortBy = null,
        string? city = null,
        Guid? categoryId = null,
        Guid? typeId = null,
        Guid? ownerId = null
    );
}
