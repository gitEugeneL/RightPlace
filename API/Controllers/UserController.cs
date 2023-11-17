using API.Models.DTOs.User;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult> Create([FromBody] UserRequestDto dto)
    {
        var result = await _userService.Create(dto);
        return Created($"api/users/{result.Id}", result);
    }

    [Authorize(Roles = "ROLE_USER")]
    [HttpGet("test")]
    public ActionResult<string> Test()
    {
        return "authorize test";
    }
}