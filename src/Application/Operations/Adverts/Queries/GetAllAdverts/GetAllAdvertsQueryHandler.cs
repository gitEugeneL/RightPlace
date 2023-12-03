using Application.Common.Interfaces;
using Application.Common.Models;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Adverts.Queries.GetAllAdverts;

public class GetAllAdvertsQueryHandler : 
    IRequestHandler<GetAllAdvertsQueryWithPagination, PaginatedList<AdvertsResponse>>
{
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;
    
    public GetAllAdvertsQueryHandler(IMapper mapper, IAdvertRepository advertRepository)
    {
        _mapper = mapper;
        _advertRepository = advertRepository;
    }
    
    public async Task<PaginatedList<AdvertsResponse>> 
        Handle(GetAllAdvertsQueryWithPagination request, CancellationToken cancellationToken)
    {
        var (advertisements, count) = await _advertRepository
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

        var advertisementsResponses = _mapper.Map<List<AdvertsResponse>>(advertisements);
        return new PaginatedList<AdvertsResponse>(
            advertisementsResponses, count, request.PageNumber, request.PageSize);
    }
}