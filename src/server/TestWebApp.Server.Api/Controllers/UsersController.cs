using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TestWebApp.Server.Api.Services;

namespace TestWebApp.Server.Api.Controllers;

[ApiController, Route("api/users")]
public class UsersController(TokenService tokenService) : ControllerBase
{
    private readonly TokenService _tokenService = tokenService;

    [Authorize, HttpGet("me")]
    public Task<IActionResult> GetMyInfo()
    {
        var result = HttpContext.User.Claims.Select(x => KeyValuePair.Create(x.Type, x.Value));
        return Task.FromResult<IActionResult>(Ok(result));
    }

    [HttpPost("login")]
    public Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        // check password here

        var token = _tokenService.GenerateToken(model.Username);
        return Task.FromResult<IActionResult>(Ok(new { Token = token }));
    }
}

public class UserLoginModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
