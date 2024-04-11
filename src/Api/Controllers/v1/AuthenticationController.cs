using Api.Helpers;
using Api.Utils;
using Application.Common.Models;
using Application.Operations.Authentications.Commands.Login;
using Application.Operations.Authentications.Commands.Logout;
using Application.Operations.Authentications.Commands.Refresh;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/auth")]
public class AuthenticationController : BaseController
{
    public AuthenticationController(IMediator mediator) : base(mediator) { }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(JwtToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<JwtToken>> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);
        CookieSetter
            .SetCookie(Response, "refreshToken", result.CookieToken.Token, result.CookieToken.Expires);
        return Ok(result.JwtToken);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(JwtToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JwtToken>> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken is null)
            return BadRequest();

        var result = await Mediator.Send(new RefreshCommand(refreshToken));
        CookieSetter
            .SetCookie(Response, "refreshToken", result.CookieToken.Token, result.CookieToken.Expires);
        return Ok(result.JwtToken);
    }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken is null)
            return BadRequest();

        var result = await Mediator.Send(new LogoutCommand(refreshToken));     
        CookieSetter.RemoveCookie(Response, "refreshToken");
        return Ok();
    }
}