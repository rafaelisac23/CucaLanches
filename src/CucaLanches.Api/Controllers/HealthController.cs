using Microsoft.AspNetCore.Mvc;

namespace CucaLanches.Api.Controllers;

[ApiController]
[Route("/health")]
public class HealthController:ControllerBase
{
    [HttpGet]

    public IActionResult Get() => Ok(new { status="ok", version="1.0.0" });
}