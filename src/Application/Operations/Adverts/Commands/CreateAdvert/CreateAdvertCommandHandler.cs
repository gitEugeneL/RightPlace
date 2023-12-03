using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;
using Type = Domain.Entities.Type;

namespace Application.Operations.Adverts.Commands.CreateAdvert;

public class CreateAdvertCommandHandler : IRequestHandler<CreateAdvertCommand, AdvertsResponse>
{
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITypeRepository _typeRepository;
    
    public CreateAdvertCommandHandler(
        IAdvertRepository advertRepository, IUserRepository userRepository,
        ICategoryRepository categoryRepository, ITypeRepository typeRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _advertRepository = advertRepository;
        _categoryRepository = categoryRepository;
        _typeRepository = typeRepository;
        _mapper = mapper;
    }
    
    public async Task<AdvertsResponse> 
        Handle(CreateAdvertCommand request, CancellationToken cancellationToken)
    {
        if (await _advertRepository.AdvertExistByTitleAsync(request.Title, cancellationToken))
            throw new AlreadyExistException(nameof(Advert), request.Title);
        
        var user = await _userRepository.FindUserByIdAsync(request.CurrentUserId, cancellationToken)
                   ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

        var category = await _categoryRepository.FindCategoryByIdAsync(request.CategoryId, cancellationToken)
                            ?? throw new NotFoundException(nameof(Category), request.CategoryId);

        var type = await _typeRepository.FindTypeByIdAsync(request.TypeId, cancellationToken)
                    ?? throw new NotFoundException(nameof(Type), request.TypeId);
        
        var advertisement = await _advertRepository.CreateAdvertAsync(
            new Advert
            {
                User = user,
                Category = category,
                Type = type,
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
            },
            cancellationToken
        );

        return _mapper.Map<AdvertsResponse>(advertisement);
    }
}