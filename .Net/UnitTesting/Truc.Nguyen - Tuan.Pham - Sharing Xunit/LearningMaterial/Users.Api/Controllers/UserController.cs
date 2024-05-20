using Microsoft.AspNetCore.Mvc;
using Users.Api.Contracts;
using Users.Api.Mappers;
using Users.Api.Models;
using Users.Api.Services;

namespace Users.Api.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        var usersResponse = users.Select(x => x.ToUserResponse());
        return Ok(usersResponse);
    }

    [HttpGet("users/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var userResponse = user.ToUserResponse();

        return Ok(userResponse);
    }

    [HttpPost("users")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest createUserRequest)
    {
        var user = new User
        {
            FullName = createUserRequest.FullName
        };

        var created = await _userService.CreateAsync(user);
        if (!created)
        {
            // Assume validation error
            return BadRequest();
        }

        var userResponse = user.ToUserResponse();

        return CreatedAtAction(nameof(GetById), new {id = userResponse.Id}, userResponse);
    }

    [HttpDelete("users/{id:guid}")]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        var deleted = await _userService.DeleteByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}
