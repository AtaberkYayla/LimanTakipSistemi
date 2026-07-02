using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipManagement.Data;

namespace ShipManagement.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AnalyticsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var summary = new
        {
            TotalShips = await _context.Ships.CountAsync(),
            TotalPorts = await _context.Ports.CountAsync(),
            TotalCrewMembers = await _context.CrewMembers.CountAsync(),
            TotalCargoes = await _context.Cargoes.CountAsync(),
            TotalVisits = await _context.ShipVisits.CountAsync(),
            TotalAssignments = await _context.ShipCrewAssignments.CountAsync()
        };
        return Ok(summary);
    }

    [HttpGet("ships-by-type")]
    public async Task<IActionResult> GetShipsByType()
    {
        var data = await _context.Ships
            .GroupBy(s => s.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("ships-by-flag")]
    public async Task<IActionResult> GetShipsByFlag()
    {
        var data = await _context.Ships
            .GroupBy(s => s.Flag)
            .Select(g => new { Flag = g.Key, Count = g.Count() })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("crew-by-role")]
    public async Task<IActionResult> GetCrewByRole()
    {
        var data = await _context.CrewMembers
            .GroupBy(c => c.Role)
            .Select(g => new { Role = g.Key, Count = g.Count() })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("visits-by-port")]
    public async Task<IActionResult> GetVisitsByPort()
    {
        var data = await _context.ShipVisits
            .Include(v => v.Port)
            .GroupBy(v => v.Port.Name)
            .Select(g => new { Port = g.Key, Count = g.Count() })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("cargo-by-type")]
    public async Task<IActionResult> GetCargoByType()
    {
        var data = await _context.Cargoes
            .GroupBy(c => c.CargoType)
            .Select(g => new
            {
                CargoType = g.Key,
                TotalWeight = g.Sum(c => c.WeightTon),
                Count = g.Count()
            })
            .ToListAsync();
        return Ok(data);
    }
}