using Application.Common.Interfaces;
using Application.Common.Models;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryWithPagination, PaginatedList<UserResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    
    public GetAllUsersQueryHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }
    
    public async Task<PaginatedList<UserResponse>> 
        Handle(GetAllUsersQueryWithPagination request, CancellationToken cancellationToken)
    {
        var users = await _userRepository
            .GetUsersWithPaginationAsync(request.PageNumber, request.PageSize, cancellationToken);
        
        var count = _userRepository.CountAllUsers();
        
        var userResponses = _mapper.Map<List<UserResponse>>(users); 
        return new PaginatedList<UserResponse>(userResponses, count, request.PageNumber, request.PageSize);
    }
}