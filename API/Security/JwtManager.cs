using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Entities;
using Microsoft.IdentityModel.Tokens;

namespace API.Security;

public interface IJwtManager
{
    string CreateToken(User user);
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
    
    
}