using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Users.Queries.GetAllUsers;

public record GetAllUsersQueryWithPagination : IRequest<PaginatedList<UserResponse>>
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; } = 1;
    
    [Range(5, 100)]
    public int PageSize { get; init; } = 10;
}