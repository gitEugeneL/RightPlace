using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Authentications.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthenticationResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtManager;
    
    public RefreshCommandHandler(IUserRepository userRepository, IJwtManager jwtManager)
    {
        _userRepository = userRepository;
        _jwtManager = jwtManager;
    }
    
    public async Task<AuthenticationResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        // find user
        var user = await _userRepository.FindUserByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (user is null)
            throw new UnauthorizedException("Refresh token isn't valid");
        // find this token in the user
        var userRefreshToken = user.RefreshTokens.First(rt => rt.Token == request.RefreshToken);

        // check refresh token expiration time
        if (userRefreshToken.Expires < DateTime.Now) 
            throw new UnauthorizedException("Refresh token is outdated"); 
        
        // remove old refresh token
        user.RefreshTokens.Remove(userRefreshToken);

        var accessToken = _jwtManager.CreateAccessToken(user);
        var refreshToken = _jwtManager.GenerateRefreshToken(user);

        user.RefreshTokens.Add(refreshToken);
        await _userRepository.UpdateUserAsync(user, cancellationToken);
        
        return new AuthenticationResponse(
            new JwtToken(accessToken),
            new CookieToken(refreshToken.Token, refreshToken.Expires)
        );
    }
}