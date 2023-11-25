using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.UserAuthentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtManager _jwtManager;
    private readonly IUserRepository _userRepository;
    
    public LoginCommandHandler(
        IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtManager jwtManager)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtManager = jwtManager;
    }
    
    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmailAsync(request.Email, cancellationToken);

        if (user is null || !_passwordHasher.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new AccessDeniedException(nameof(User), request.Email);
       
        // each user can only have 5  refresh tokens
        if (user.RefreshTokens.Count >= 5) 
        {
            var oldestToken = user.RefreshTokens.OrderBy(rt => rt.Created).First();
            user.RefreshTokens.Remove(oldestToken);
        }
        
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