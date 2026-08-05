namespace RacehubApi.DTOs;

public record ParticipantDto
{
    public int Id { get; init; }
    public int Dorsal { get; init; }
    public string? Time { get; init; }
    public bool Banned { get; init; }
    
    // Nested objects to replicate Symfony's "trail_running_participant:read"
    public UserDto? User { get; init; }
    public RaceDto? TrailRunning { get; init; }
}

public record CreateParticipantDto(int TrailRunning, int User);
public record UpdateParticipantDto(int? Dorsal, string? Time, bool? Banned);
