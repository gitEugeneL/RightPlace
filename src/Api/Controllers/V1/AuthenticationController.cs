using Api.Utils;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(JwtToken), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] AuthRequestDto dto)
    {
        var result = await _authenticationService.Login(dto);
        CookieSetter
            .SetCookie(Response, "refreshToken", result.CookieToken.Token, result.CookieToken.Expires);
        return Ok(result.JwtToken);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh()
    {
        var result = await _authenticationService.Refresh(Request.Cookies["refreshToken"]);
        CookieSetter
            .SetCookie(Response, "refreshToken", result.CookieToken.Token, result.CookieToken.Expires);
        return Ok(result.JwtToken);
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await _authenticationService.Logout(Request.Cookies["refreshToken"]);
        CookieSetter
            .RemoveCookie(Response,"refreshToken");
        return Ok();
    }
}