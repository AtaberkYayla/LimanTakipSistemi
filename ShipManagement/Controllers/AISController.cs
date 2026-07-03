using Microsoft.AspNetCore.Mvc;
using ShipManagement.Services;

namespace ShipManagement.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AISController : ControllerBase
{
    [HttpGet("live-ships")]
    public IActionResult GetLiveShips()
    {
        var ships = AISStreamService.GetRecentShips();
        return Ok(ships);
    }
}