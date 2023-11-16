using API.Exceptions;
using API.Models;
using API.Models.DTOs.Auth;
using API.Repositories;
using API.Security;

namespace API.Services;

public interface IAuthenticationService
{
    Task<Token> Login(AuthRequestDto dto);
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
    
    public async Task<Token> Login(AuthRequestDto dto)
    {
        var user = await _userRepository.FindUserByEmailAsync(dto.Email);
        if (user is null || !_passwordHasher.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            throw new NotFoundException($"User '{dto.Email}' doesn't exist or your password is incorrect");
        
        var accessToken = _jwtManager.CreateToken(user);
        return new Token { Type = "Bearer", AccessToken = accessToken };
    }
}