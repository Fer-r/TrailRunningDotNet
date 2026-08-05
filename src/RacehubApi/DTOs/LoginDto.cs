using System.ComponentModel.DataAnnotations;

namespace RacehubApi.DTOs;

public record LoginDto(
    [Required] string Email, 
    [Required] string Password
);
