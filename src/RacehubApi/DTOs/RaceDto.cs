using System.Text.Json.Serialization;

namespace RacehubApi.DTOs;

public record RaceDto(
    int Id,
    string Name,
    string Description,
    DateTime Date,
    [property: JsonPropertyName("distance_km")] int DistanceKm,
    string Location,
    string? Coordinates,
    int? Unevenness,
    [property: JsonPropertyName("entry_fee")] int EntryFee,
    [property: JsonPropertyName("available_slots")] int AvailableSlots,
    string Status,
    string? Category,
    [property: JsonPropertyName("img")] string? Image,
    string? Gender,
    [property: JsonPropertyName("trailRunningParticipants")] IReadOnlyList<ParticipantDto> TrailRunningParticipants
);

public record ParticipantDto(
    int Id,
    int UserId,
    int TrailRunningId,
    int Dorsal,
    string? Time,
    bool Banned,
    UserSimpleDto User
);

public record UserSimpleDto(
    int Id,
    string Name
);
