namespace Application.Common.Models.DTOs.Auth;

public record AuthResponseDto(JwtToken JwtToken, CookieToken CookieToken);