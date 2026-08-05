namespace RacehubApi.DTOs;

public record EditUserRequest(
    string? Name,
    string? Oldpassword,
    string? Newpassword
);
