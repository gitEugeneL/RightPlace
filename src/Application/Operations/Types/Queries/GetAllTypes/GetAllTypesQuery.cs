using MediatR;

namespace Application.Operations.Types.Queries.GetAllTypes;

public record GetAllTypesQuery : IRequest<IReadOnlyCollection<TypeResponse>>; 
