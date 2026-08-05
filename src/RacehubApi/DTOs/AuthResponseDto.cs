namespace RacehubApi.DTOs;

public record UserDto(
    int Id,
    string Email,
    string Name,
    string Roles,
    int? Age,
    string? Gender,
    string? Image,
    bool Banned
);

public record AuthResponseDto(
    string Token,
    UserDto User
);
