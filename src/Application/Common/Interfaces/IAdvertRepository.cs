using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAdvertRepository
{
    Task<bool> AdvertExistByTitleAsync(string title, CancellationToken cancellationToken);

    Task UpdateAdvertAsync(Advert advert, CancellationToken cancellationToken);

    Task DeleteAdvertAsync(Advert advert, CancellationToken cancellationToken);
    
    Task<Advert> CreateAdvertAsync(Advert advert, CancellationToken cancellationToken);

    Task<Advert?> FindAdvertByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<(IEnumerable<Advert> List, int Count)> GetAdvertWithPaginationAsync(
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
