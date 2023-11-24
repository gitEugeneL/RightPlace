using Application.Users;
using Application.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/users")]
public class UserController : BaseController
{
    public UserController(IMediator mediator) : base(mediator) { }
    
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserCommand command)
    {
        var result = await Mediator.Send(command);
        return Created($"api/users/{result.Id}", result);
    }
}