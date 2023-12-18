using MediatR;

namespace Application.Operations.Authentications.Commands.Refresh;

public record RefreshCommand(string RefreshToken) : IRequest<AuthenticationResponse>;