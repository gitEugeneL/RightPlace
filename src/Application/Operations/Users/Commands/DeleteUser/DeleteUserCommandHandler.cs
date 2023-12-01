using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    
    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.FindUserByIdAsync(request.CurrentUserId, cancellationToken)
                    ?? throw new NotFoundException(nameof(user), request.CurrentUserId);

        await _userRepository.DeleteUserAsync(user, cancellationToken);
        return await Unit.Task;
    }
}