using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RacehubApi.Data;
using RacehubApi.DTOs;
using RacehubApi.Models;

namespace RacehubApi.Services;

public class AuthService(RacehubContext context, IConfiguration configuration)
{
    public async Task<AuthResponseDto?> AuthenticateAsync(LoginDto loginDto)
    {
        var user = await context.Users
            .Include(u => u.TrailRunningParticipants)
                .ThenInclude(p => p.TrailRunning)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            return null;
        }

        if (user.Banned)
        {
            throw new UnauthorizedAccessException("El usuario está baneado.");
        }

        string token = GenerateJwtToken(user);
        
        var userDto = new UserDto(
            user.Id,
            user.Email,
            user.Name,
            user.Roles,
            user.Age,
            user.Gender,
            user.Image,
            user.Banned,
            user.TrailRunningParticipants.Select(p => new ParticipantDto
            {
                Id = p.Id,
                Dorsal = p.Dorsal,
                Time = p.Time,
                Banned = p.Banned,
                TrailRunning = new RaceDto(
                    p.TrailRunning.Id,
                    p.TrailRunning.Name,
                    p.TrailRunning.Description,
                    p.TrailRunning.Date,
                    p.TrailRunning.DistanceKm,
                    p.TrailRunning.Location,
                    p.TrailRunning.Coordinates,
                    p.TrailRunning.Unevenness,
                    p.TrailRunning.EntryFee,
                    p.TrailRunning.AvailableSlots,
                    p.TrailRunning.Status,
                    p.TrailRunning.Category,
                    p.TrailRunning.Image,
                    p.TrailRunning.Gender,
                    new List<RaceParticipantDto>()
                )
            }).ToList()
        );

        return new AuthResponseDto(token, userDto);
    }

    private string GenerateJwtToken(User user)
    {
        string secretKey = configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey missing");
        string issuer = configuration["Jwt:Issuer"] ?? "RacehubApi";
        string audience = configuration["Jwt:Audience"] ?? "RacehubWeb";

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name)
        };
        
        // El campo Roles tiene un formato string serializado JSON, e.g. ["ROLE_USER"]
        // Lo parsearemos de manera sencilla para añadir roles al token.
        if (!string.IsNullOrEmpty(user.Roles))
        {
            var cleanRoles = user.Roles
                .Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
            
            var rolesList = cleanRoles.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in rolesList)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}
