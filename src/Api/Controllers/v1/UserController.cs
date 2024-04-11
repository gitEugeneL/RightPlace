using Api.Helpers;
using Application.Common.Models;
using Application.Operations.Users;
using Application.Operations.Users.Commands.CreateUser;
using Application.Operations.Users.Commands.DeleteUser;
using Application.Operations.Users.Commands.UpdateUser;
using Application.Operations.Users.Queries.GetAllUsers;
using Application.Operations.Users.Queries.GetUser;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/users")]
public class UserController : BaseController
{
    public UserController(IMediator mediator) : base(mediator) { }
    
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserCommand command)
    {
        var result = await Mediator.Send(command);
        return Created($"api/users/{result.Id}", result);
    }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<UserResponse>>> GetAll([FromQuery] GetAllUsersQueryWithPagination query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetOne(Guid id)
    {
        var result = await Mediator.Send(new GetUserQuery(id));
        return Ok(result);
    }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpPut]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> Update([FromBody] UpdateUserCommand command)
    {
        command.SetCurrentUserId(CurrentUserId());
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete()
    {
        await Mediator.Send(new DeleteUserCommand()
            .SetCurrentUserId(CurrentUserId()));
        return NoContent();
    }
}