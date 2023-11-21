using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.DTOs.Auth;
using Domain.Entities;

namespace Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtManager _jwtManager;
    
    public AuthenticationService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtManager jwtManager)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtManager = jwtManager;
    }
    
    public async Task<AuthResponseDto> Login(AuthRequestDto dto)
    {
        var user = await _userRepository.FindUserByEmailAsync(dto.Email);
        if (user is null || !_passwordHasher.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            throw new NotFoundException($"User '{dto.Email}' doesn't exist or your password is incorrect");
        
        if (user.RefreshTokens.Count >= 5) // each user can only have 5  refresh tokens
        {
            var oldestToken = user.RefreshTokens.OrderBy(rt => rt.Created).First();
            user.RefreshTokens.Remove(oldestToken);
        }
        
        var accessToken = _jwtManager.CreateAccessToken(user);
        var refreshToken = _jwtManager.GenerateRefreshToken(user);
        
        user.RefreshTokens.Add(refreshToken);
        await _userRepository.UpdateUserAsync(user);
        
        return new AuthResponseDto(new JwtToken(accessToken), new CookieToken(refreshToken.Token, refreshToken.Expires));
    }

    public async Task<AuthResponseDto> Refresh(string? requestRefreshToken)
    {
        var user = await ValidateAndRemoveRefreshToken(requestRefreshToken);

        var accessToken = _jwtManager.CreateAccessToken(user);
        var refreshToken = _jwtManager.GenerateRefreshToken(user);

        user.RefreshTokens.Add(refreshToken);
        await _userRepository.UpdateUserAsync(user);
        
        return new AuthResponseDto(new JwtToken(accessToken), new CookieToken(refreshToken.Token, refreshToken.Expires));
    }

    public async Task Logout(string? requestRefreshToken)
    {
        var user = await ValidateAndRemoveRefreshToken(requestRefreshToken);
        await _userRepository.UpdateUserAsync(user);
    }
    
    private async Task<User> ValidateAndRemoveRefreshToken(string? requestRefreshToken)
    {
        if (requestRefreshToken is null)
            throw new UnauthorizedException("Refresh token doesn't exist");
        
        var user = await _userRepository.FindUserByRefreshTokenAsync(requestRefreshToken);
        if (user is null)
            throw new UnauthorizedException("Refresh token isn't valid");
        
        var userRefreshToken = user.RefreshTokens.First(rt => rt.Token == requestRefreshToken);
        
        // check refresh token expiration time
        if (userRefreshToken.Expires < DateTime.Now) 
            throw new UnauthorizedException("Refresh token is outdated");
        
        // remove the old refresh token
        user.RefreshTokens.Remove(userRefreshToken); 
        
        return user;
    }
}