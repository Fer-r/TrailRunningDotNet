using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RacehubApi.DTOs;
using RacehubApi.Services;

namespace RacehubApi.Controllers;

[ApiController]
[Route("api/trailrunning_participant")]
public class ParticipantController : ControllerBase
{
    private readonly IParticipantService _participantService;

    public ParticipantController(IParticipantService participantService)
    {
        _participantService = participantService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ParticipantDto>>> GetAll()
    {
        var participants = await _participantService.GetAllAsync();
        return Ok(participants);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParticipantDto>> GetById(int id)
    {
        var participant = await _participantService.GetByIdAsync(id);
        if (participant == null) return NotFound(new { error = "Participant not found" });
        return Ok(participant);
    }

    [HttpPost("new")]
    [Authorize]
    public async Task<ActionResult<ParticipantDto>> Register([FromBody] CreateParticipantDto request)
    {
        try 
        {
            var participant = await _participantService.RegisterAsync(request);
            if (participant == null) return BadRequest(new { error = "Invalid race or user" });
            
            return CreatedAtAction(nameof(GetById), new { id = participant.Id }, participant);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}/edit")]
    // [Authorize] -> The original backend didn't authorize this specifically or if it did, the React client might not send token? Actually it does send token. I'll just leave it open for now or match plan.
    // Actually the plan says: [Authorize] en POST y DELETE. PUT is "-".
    public async Task<ActionResult<ParticipantDto>> Update(int id, [FromBody] UpdateParticipantDto request)
    {
        var participant = await _participantService.UpdateAsync(id, request);
        if (participant == null) return NotFound(new { error = "Participant not found" });
        return Ok(participant);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _participantService.DeleteAsync(id);
        if (!deleted) return NotFound(new { error = "Participant not found" });
        return Ok(true); // Return JSON `true` like Symfony
    }
}
