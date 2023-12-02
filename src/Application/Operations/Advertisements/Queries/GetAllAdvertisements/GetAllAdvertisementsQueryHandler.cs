using Application.Common.Interfaces;
using Application.Common.Models;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Advertisements.Queries.GetAllAdvertisements;

public class GetAllAdvertisementsQueryHandler : 
    IRequestHandler<GetAllAdvertisementsQueryWithPagination, PaginatedList<AdvertisementResponse>>
{
    private readonly IMapper _mapper;
    private readonly IAdvertisementRepository _advertisementRepository;
    
    public GetAllAdvertisementsQueryHandler(IMapper mapper, IAdvertisementRepository advertisementRepository)
    {
        _mapper = mapper;
        _advertisementRepository = advertisementRepository;
    }
    
    public async Task<PaginatedList<AdvertisementResponse>> 
        Handle(GetAllAdvertisementsQueryWithPagination request, CancellationToken cancellationToken)
    {
        var (advertisements, count) = await _advertisementRepository
            .GetAdvertisementWithPaginationAsync(
                cancellationToken: cancellationToken,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                sortOrderAsc: request.SortOrderAsc,
                sortBy: request.SortBy,
                city: request.City,
                categoryId: request.CategoryId,
                typeId: request.TypeId,
                ownerId: request.OwnerId
            );

        var advertisementsResponses = _mapper.Map<List<AdvertisementResponse>>(advertisements);
        return new PaginatedList<AdvertisementResponse>(
            advertisementsResponses, count, request.PageNumber, request.PageSize);
    }
}