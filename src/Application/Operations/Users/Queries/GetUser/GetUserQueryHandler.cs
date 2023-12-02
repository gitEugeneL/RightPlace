using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public GetUserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<UserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByIdAsync(request.Id, cancellationToken)
                   ?? throw new NotFoundException(nameof(User), request.Id);
        
        return _mapper.Map<UserResponse>(user);
    }
}
