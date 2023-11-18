using API.Entities;
using API.Exceptions;
using API.Models;
using API.Models.DTOs.Auth;
using API.Repositories;
using API.Security;

namespace API.Services;

public interface IAuthenticationService
{
    Task<Token> Login(HttpResponse response, AuthRequestDto dto);
    Task<Token> Refresh(HttpResponse response, string? requestRefreshToken);
    Task Logout(HttpResponse response, string? requestRefreshToken);
}

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
    
    public async Task<Token> Login(HttpResponse response, AuthRequestDto dto)
    {
        var user = await _userRepository.FindUserByEmailAsync(dto.Email);
        if (user is null || !_passwordHasher.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            throw new NotFoundException($"User '{dto.Email}' doesn't exist or your password is incorrect");
        
        if (user.RefreshTokens.Count >= 5) // each user can only have 5  refresh tokens
        {
            var oldestToken = user.RefreshTokens.OrderBy(rt => rt.CreatedDate).First();
            user.RefreshTokens.Remove(oldestToken);
        }
        
        var accessToken = _jwtManager.CreateToken(user);
        var refreshToken = _jwtManager.GenerateRefreshToken(user);
        
        user.RefreshTokens.Add(refreshToken);
        await _userRepository.UpdateUserAsync(user);
        
        _jwtManager.SetRefreshTokenToCookies(response, refreshToken);
        return new Token { Type = "Bearer", AccessToken = accessToken };
    }

    public async Task<Token> Refresh(HttpResponse response, string? requestRefreshToken)
    {
        var user = await ValidateAndRemoveRefreshToken(requestRefreshToken);

        var accessToken = _jwtManager.CreateToken(user);
        var refreshToken = _jwtManager.GenerateRefreshToken(user);

        user.RefreshTokens.Add(refreshToken);
        await _userRepository.UpdateUserAsync(user);

        _jwtManager.SetRefreshTokenToCookies(response, refreshToken);
        return new Token { Type = "Bearer", AccessToken = accessToken };
    }

    public async Task Logout(HttpResponse response, string? requestRefreshToken)
    {
        var user = await ValidateAndRemoveRefreshToken(requestRefreshToken);
        _jwtManager.DeleteRefreshTokenFromCookie(response);
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