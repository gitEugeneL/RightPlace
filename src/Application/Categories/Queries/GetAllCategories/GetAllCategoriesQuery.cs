using MediatR;

namespace Application.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery : IRequest<IReadOnlyCollection<CategoryResponse>>;