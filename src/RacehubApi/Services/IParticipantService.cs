using RacehubApi.DTOs;
using RacehubApi.Models;

namespace RacehubApi.Services;

public interface IParticipantService
{
    Task<IReadOnlyList<ParticipantDto>> GetAllAsync();
    Task<ParticipantDto?> GetByIdAsync(int id);
    Task<ParticipantDto?> RegisterAsync(CreateParticipantDto dto);
    Task<ParticipantDto?> UpdateAsync(int id, UpdateParticipantDto dto);
    Task<bool> DeleteAsync(int id);
}
