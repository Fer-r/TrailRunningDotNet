using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace RacehubApi.DTOs;

public record RaceCreateDto(
    [Required] string Name,
    string Description,
    DateTime Date,
    [property: JsonPropertyName("distance_km")] int DistanceKm,
    [Required] string Location,
    string? Coordinates,
    int? Unevenness,
    [property: JsonPropertyName("entry_fee")] int EntryFee,
    [property: JsonPropertyName("available_slots")] int AvailableSlots,
    string Status,
    string? Category,
    [property: JsonPropertyName("img")] string? Image,
    string? Gender
);

public record RaceUpdateDto(
    string? Name,
    string? Description,
    DateTime? Date,
    [property: JsonPropertyName("distance_km")] int? DistanceKm,
    string? Location,
    string? Coordinates,
    int? Unevenness,
    [property: JsonPropertyName("entry_fee")] int? EntryFee,
    [property: JsonPropertyName("available_slots")] int? AvailableSlots,
    string? Status,
    string? Category,
    [property: JsonPropertyName("img")] string? Image,
    string? Gender
);
