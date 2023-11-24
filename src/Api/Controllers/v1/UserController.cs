using Application.Users;
using Application.Users.Commands;
using Application.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Created($"api/users/{result.Id}", result);
    }
}