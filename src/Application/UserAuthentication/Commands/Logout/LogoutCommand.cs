using MediatR;

namespace Application.UserAuthentication.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<Unit>
{
    public string RefreshToken { get; set; } = RefreshToken;
}