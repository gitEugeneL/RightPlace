using Application.Common.Models;

namespace Application.Operations.Authentications;

public record AuthenticationResponse(
    JwtToken JwtToken, 
    CookieToken CookieToken
);