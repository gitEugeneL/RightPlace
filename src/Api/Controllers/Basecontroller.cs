using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IMediator Mediator;
    
    public BaseController(IMediator mediator) {
        Mediator = mediator;
    }

    protected string? CurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}