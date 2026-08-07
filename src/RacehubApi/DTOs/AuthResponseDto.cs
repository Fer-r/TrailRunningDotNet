using System.Text.Json.Serialization;

namespace RacehubApi.DTOs;

public record UserDto(
    int Id,
    string Email,
    string Name,
    string Roles,
    int? Age,
    string? Gender,
    string? Image,
    bool Banned,
    [property: JsonPropertyName("trailRunningParticipants")] IReadOnlyList<ParticipantDto> TrailRunningParticipants
);

public record AuthResponseDto(
    string Token,
    UserDto User
);
