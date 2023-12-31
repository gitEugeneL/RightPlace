using MediatR;

namespace Application.Operations.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Unit>
{
    public Guid CurrentUserId { get; private set; }
    
    public DeleteUserCommand SetCurrentUserId(string id)
    {
        CurrentUserId = Guid.Parse(id);
        return this;
    }
}