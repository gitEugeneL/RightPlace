using Application.Common.Models;
using Application.Operations.Users;
using Application.Operations.Users.Commands.CreateUser;
using Application.Operations.Users.Commands.DeleteUser;
using Application.Operations.Users.Commands.UpdateUser;
using Application.Operations.Users.Queries.GetAllUsers;
using Application.Operations.Users.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<UserResponse>>> GetAll([FromQuery] GetAllUsersQueryWithPagination query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> GetOne(Guid id)
    {
        var result = await Mediator.Send(new GetUserQuery(id));
        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> Update([FromBody] UpdateUserCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();

        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete()
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();
        
        await Mediator.Send(new DeleteUserCommand().SetCurrentUserId(id));
        return Ok();
    }
}