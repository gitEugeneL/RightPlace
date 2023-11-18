using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using API.Entities;
using Microsoft.IdentityModel.Tokens;

namespace API.Security;

public interface IJwtManager
{
    string CreateToken(User user);
    RefreshToken GenerateRefreshToken(User user);
    void SetRefreshTokenToCookies(HttpResponse response, RefreshToken refreshToken);
    void DeleteRefreshTokenFromCookie(HttpResponse response);
}

public sealed class JwtManager : IJwtManager
{
    private readonly IConfiguration _configuration;

    public JwtManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.Value)
        };

        var settings = _configuration.GetSection("Authentication:Key").Value!;
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(settings));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(
                int.Parse(_configuration.GetSection("Authentication:TokenLifetimeMin").Value!)),
            SigningCredentials = credentials
        };
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);
        
        return handler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(User user)
    {
        return new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(256)),
            Expires = DateTime.Now.AddDays(
                int.Parse(_configuration.GetSection("Authentication:RefreshTokenLifetimeDays").Value!)),
            User = user
        };
    }
    
    public void SetRefreshTokenToCookies(HttpResponse response, RefreshToken refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshToken.Expires,
        };
        response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }

    public void DeleteRefreshTokenFromCookie(HttpResponse response)
    {
        response.Cookies.Delete("refreshToken");
    }
}