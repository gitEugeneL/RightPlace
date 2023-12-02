using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;
using Type = Domain.Entities.Type;

namespace Application.Operations.Advertisements.Commands.CreateAdvertisement;

public class CreateAdvertisementCommandHandler : IRequestHandler<CreateAdvertisementCommand, AdvertisementResponse>
{
    private readonly IMapper _mapper;
    private readonly IAdvertisementRepository _advertisementRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITypeRepository _typeRepository;
    
    public CreateAdvertisementCommandHandler(
        IAdvertisementRepository advertisementRepository, IUserRepository userRepository,
        ICategoryRepository categoryRepository, ITypeRepository typeRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _advertisementRepository = advertisementRepository;
        _categoryRepository = categoryRepository;
        _typeRepository = typeRepository;
        _mapper = mapper;
    }
    
    public async Task<AdvertisementResponse> 
        Handle(CreateAdvertisementCommand request, CancellationToken cancellationToken)
    {
        if (await _advertisementRepository.AdvertisementExistByTitleAsync(request.Title, cancellationToken))
            throw new AlreadyExistException(nameof(Advertisement), request.Title);
        
        var user = await _userRepository.FindUserByIdAsync(request.CurrentUserId, cancellationToken)
                   ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

        var category = await _categoryRepository.FindCategoryByIdAsync(request.CategoryId, cancellationToken)
                            ?? throw new NotFoundException(nameof(Category), request.CategoryId);

        var type = await _typeRepository.FindTypeByIdAsync(request.TypeId, cancellationToken)
                    ?? throw new NotFoundException(nameof(Type), request.TypeId);
        
        var advertisement = await _advertisementRepository.CreateAdvertisementAsync(
            new Advertisement
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

        return _mapper.Map<AdvertisementResponse>(advertisement);
    }
}