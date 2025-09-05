using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controllers.System.System;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { pong = true, utc = DateTime.UtcNow });
}
