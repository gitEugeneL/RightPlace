using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAdvertisementRepository
{
    Task<bool> AdvertisementExistByTitleAsync(string title, CancellationToken cancellationToken);

    Task UpdateAdvertisementAsync(Advertisement advertisement, CancellationToken cancellationToken);

    Task DeleteAdvertisementAsync(Advertisement advertisement, CancellationToken cancellationToken);
    
    Task<Advertisement> CreateAdvertisementAsync(Advertisement advertisement, CancellationToken cancellationToken);

    Task<Advertisement?> FindAdvertisementByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<(IEnumerable<Advertisement> List, int Count)> GetAdvertisementWithPaginationAsync(
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
