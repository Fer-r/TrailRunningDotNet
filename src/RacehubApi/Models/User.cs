using System.ComponentModel.DataAnnotations;

namespace RacehubApi.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    public required string Email { get; set; }
    
    [Required]
    public required string Password { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    public string Roles { get; set; } = "[\"ROLE_USER\"]";
    public int? Age { get; set; }
    public string? Gender { get; set; }
    public string? Image { get; set; }
    public bool Banned { get; set; }

    public List<TrailRunningParticipant> TrailRunningParticipants { get; set; } = [];
}
