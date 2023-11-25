using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IJwtManager
{
    string CreateAccessToken(User user);
    
    RefreshToken GenerateRefreshToken(User user);
}