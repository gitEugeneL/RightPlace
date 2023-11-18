using API.Models;
using API.Models.DTOs.Auth;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

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
    [ProducesResponseType(typeof(Token), StatusCodes.Status200OK)]
    public async Task<ActionResult> Login([FromBody] AuthRequestDto dto)
    {
        var result = await _authenticationService.Login(Response, dto);
        return Ok(result);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Refresh()
    {
        var result = await _authenticationService.Refresh(Response, Request.Cookies["refreshToken"]);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Logout()
    {
        await _authenticationService.Logout(Response, Request.Cookies["refreshToken"]);
        return Ok();
    }
}