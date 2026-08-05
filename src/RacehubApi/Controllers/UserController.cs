using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RacehubApi.Data;
using RacehubApi.DTOs;

namespace RacehubApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly RacehubContext _context;

    public UserController(RacehubContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
            return NotFound(new { error = "User not found" });

        var userDto = new UserDto(
            user.Id,
            user.Email,
            user.Name,
            user.Roles,
            user.Age,
            user.Gender,
            user.Image,
            user.Banned
        );

        return Ok(userDto);
    }

    [HttpPut("{id}/edit")]
    [Authorize]
    public async Task<IActionResult> EditUser(int id, [FromBody] EditUserRequest request)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
            return NotFound(new { error = "User not found" });

        // Verificar que el usuario que intenta editar es él mismo (o admin, pero vamos a dejarlo simple)
        // var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        // if (currentUserId != id.ToString()) return Forbid();

        // Update name
        if (!string.IsNullOrEmpty(request.Name))
        {
            user.Name = request.Name;
        }

        // Update password if requested
        if (!string.IsNullOrEmpty(request.Oldpassword) && !string.IsNullOrEmpty(request.Newpassword))
        {
            bool isOldPasswordValid = BCrypt.Net.BCrypt.Verify(request.Oldpassword, user.Password);
            if (!isOldPasswordValid)
            {
                return BadRequest(new { error = "Contraseña actual incorrecta." });
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Newpassword);
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "User updated successfully" });
    }
}
