using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.UserAuthentication.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    
    public LogoutCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // find user
        var user = await _userRepository.FindUserByRefreshTokenAsync(request.RefreshToken, cancellationToken)
            ?? throw new UnauthorizedException("Refresh token isn't valid"); 
            
        // find this token in the user
        var userRefreshToken = user.RefreshTokens.First(rt => rt.Token == request.RefreshToken);

        // check refresh token expiration time
        if (userRefreshToken.Expires < DateTime.Now) 
            throw new UnauthorizedException("Refresh token is outdated"); 
        
        // remove old refresh token
        user.RefreshTokens.Remove(userRefreshToken);
        
        await _userRepository.UpdateUserAsync(user, cancellationToken);

        return await Unit.Task;
    }
}