using MediatR;

namespace Application.Operations.Authentications.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<Unit>;
