using MediatR;

namespace Application.Operations.Users.Queries.GetUser;

public record GetUserQuery(Guid Id) : IRequest<UserResponse>;
