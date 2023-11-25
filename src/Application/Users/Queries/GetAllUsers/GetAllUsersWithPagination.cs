using Application.Common.Models;
using MediatR;

namespace Application.Users.Queries.GetAllUsers;

public class GetAllUsersWithPagination : IRequest<PaginatedList<UserResponse>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}