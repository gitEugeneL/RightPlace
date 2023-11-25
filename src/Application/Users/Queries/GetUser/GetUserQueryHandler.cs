using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MapsterMapper;
using MediatR;

namespace Application.Users.Queries.GetUser;

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
        var user = await _userRepository.GetUserById(request.Id);
        if (user is null)
            throw new NotFoundException(nameof(user), request.Id);
        
        return _mapper.Map<UserResponse>(user);
    }
}
