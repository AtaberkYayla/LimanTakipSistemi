using Microsoft.AspNetCore.Mvc;
using ShipManagement.DTOs;
using ShipManagement.Services;

namespace ShipManagement.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CrewMembersController : ControllerBase
{
    private readonly ICrewMemberService _crewMemberService;

    public CrewMembersController(ICrewMemberService crewMemberService)
    {
        _crewMemberService = crewMemberService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _crewMemberService.GetAllAsync();
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _crewMemberService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCrewMemberDto dto)
    {
        var result = await _crewMemberService.AddAsync(dto);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value!.CrewId }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCrewMemberDto dto)
    {
        var result = await _crewMemberService.UpdateAsync(id, dto);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _crewMemberService.DeleteAsync(id);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }
}