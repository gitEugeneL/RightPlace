using Application.Common.Interfaces;
using MapsterMapper;
using MediatR;

namespace Application.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IReadOnlyCollection<CategoryResponse>>
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyCollection<CategoryResponse>> Handle(GetAllCategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllCategories(cancellationToken);
        return _mapper.Map<IReadOnlyCollection<CategoryResponse>>(categories);
    }
}