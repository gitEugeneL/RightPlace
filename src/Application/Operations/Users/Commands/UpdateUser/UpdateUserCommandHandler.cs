using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponse>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    
    public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }
    
    public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.FindUserByIdAsync(request.CurrentUserId, cancellationToken)
                   ?? throw new NotFoundException(nameof(user), request.CurrentUserId);
    
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Phone = request.Phone ?? user.Phone;
        user.DateOfBirth = request.DateOfBirth ?? user.DateOfBirth;
        
        await _userRepository.UpdateUserAsync(user, cancellationToken);
        return _mapper.Map<UserResponse>(user);
    }
}