using Application.Common.Interfaces;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Types.Queries.GetAllTypes;

public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IReadOnlyCollection<TypeResponse>>
{
    private readonly IMapper _mapper;
    private readonly ITypeRepository _typeRepository;

    public GetAllTypesQueryHandler(IMapper mapper, ITypeRepository typeRepository)
    {
        _mapper = mapper;
        _typeRepository = typeRepository;
    }

    public async Task<IReadOnlyCollection<TypeResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await _typeRepository.GetAllTypesAsync(cancellationToken);
        return _mapper.Map<IReadOnlyCollection<TypeResponse>>(types);
    }
}