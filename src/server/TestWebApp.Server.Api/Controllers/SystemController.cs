using Microsoft.AspNetCore.Mvc;

namespace TestWebApp.Server.Api.Controllers;

[ApiController, Route("api/system")]
public class SystemController : ControllerBase
{
    [HttpGet("ping")]
    public Task<IActionResult> Ping()
    {
        return Task.FromResult<IActionResult>(Ok(new { Message = "Ok" }));
    }

    [HttpPost("echo")]
    public Task<IActionResult> Echo([FromBody] dynamic body)
    {
        return Task.FromResult<IActionResult>(Ok(body));
    }
}
