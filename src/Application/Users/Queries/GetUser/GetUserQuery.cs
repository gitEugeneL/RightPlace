using MediatR;

namespace Application.Users.Queries.GetUser;

public record GetUserQuery(Guid Id) : IRequest<UserResponse>;
