using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RacehubApi.DTOs;
using RacehubApi.Services;

namespace RacehubApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrailRunningController(RaceService raceService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RaceDto>>> GetAll()
    {
        var races = await raceService.GetAllAsync();
        return Ok(races);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RaceDto>> GetById(int id)
    {
        var race = await raceService.GetByIdAsync(id);
        if (race == null)
            return NotFound(new { message = "Carrera no encontrada" });

        return Ok(race);
    }

    [Authorize]
    [HttpPost("new")]
    public async Task<ActionResult<RaceDto>> Create([FromBody] RaceCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdRace = await raceService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdRace.Id }, createdRace);
    }

    [Authorize]
    [HttpPut("{id}/edit")]
    public async Task<ActionResult<RaceDto>> Update(int id, [FromBody] RaceUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedRace = await raceService.UpdateAsync(id, dto);
        if (updatedRace == null)
            return NotFound(new { message = "Carrera no encontrada" });

        return Ok(updatedRace);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await raceService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Carrera no encontrada" });

        return NoContent();
    }
}
