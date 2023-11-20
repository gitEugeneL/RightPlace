using Application.Common.Models.DTOs.Auth;

namespace Application.Common.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResponseDto> Login(AuthRequestDto dto);
    Task<AuthResponseDto> Refresh(string? requestRefreshToken);
    Task Logout(string? requestRefreshToken);
}