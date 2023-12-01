using MediatR;

namespace Application.Operations.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery : IRequest<IReadOnlyCollection<CategoryResponse>>;