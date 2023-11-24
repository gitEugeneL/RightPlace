using Application.Common.Models;

namespace Application.UserAuthentication;

public record AuthenticationResponse(
    JwtToken JwtToken, 
    CookieToken CookieToken
);