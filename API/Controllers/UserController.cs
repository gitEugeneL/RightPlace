using API.Models.DTOs;
using API.Services;
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
        Console.WriteLine(dto);
        
        var result = await _userService.Create(dto);
        return Created($"api/users/{result.Id}", result);
    }
}