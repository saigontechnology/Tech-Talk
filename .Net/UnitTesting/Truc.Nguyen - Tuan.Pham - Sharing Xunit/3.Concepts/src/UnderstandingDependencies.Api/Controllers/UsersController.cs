using Microsoft.AspNetCore.Mvc;
using UnderstandingDependencies.Api.Repositories;
using UnderstandingDependencies.Api.Services;

namespace UnderstandingDependencies.Api.Controllers;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController()
    {
        _userService = new UserService(new UserRepository());
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }
}
