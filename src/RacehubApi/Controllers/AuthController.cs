using Microsoft.AspNetCore.Mvc;
using RacehubApi.DTOs;
using RacehubApi.Services;

namespace RacehubApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await authService.AuthenticateAsync(loginDto);
            
            if (response == null)
            {
                // To prevent user enumeration, we return the same generic error
                return Unauthorized(new { message = "Credenciales incorrectas" });
            }

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message }); // Forbidden if banned
        }
    }
}
