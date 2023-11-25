using MediatR;

namespace Application.UserAuthentication.Commands.Refresh;

public record RefreshCommand(string RefreshToken) : IRequest<AuthenticationResponse>
{
    public string RefreshToken { get; set; } = RefreshToken;
}