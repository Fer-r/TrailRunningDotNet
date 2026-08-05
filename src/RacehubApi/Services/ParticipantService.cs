using Microsoft.EntityFrameworkCore;
using RacehubApi.Data;
using RacehubApi.DTOs;
using RacehubApi.Models;

namespace RacehubApi.Services;

public partial class ParticipantService : IParticipantService
{
    private readonly RacehubContext _context;

    public ParticipantService(RacehubContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ParticipantDto>> GetAllAsync()
    {
        return await _context.TrailRunningParticipants
            .AsNoTracking()
            .Select(p => new ParticipantDto
            {
                Id = p.Id,
                Dorsal = p.Dorsal,
                Time = p.Time,
                Banned = p.Banned,
                User = new UserDto(
                    p.User.Id,
                    p.User.Email,
                    p.User.Name,
                    p.User.Roles,
                    p.User.Age,
                    p.User.Gender,
                    p.User.Image,
                    p.User.Banned
                ),
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
            })
            .ToListAsync();
    }

    public async Task<ParticipantDto?> GetByIdAsync(int id)
    {
        return await _context.TrailRunningParticipants
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ParticipantDto
            {
                Id = p.Id,
                Dorsal = p.Dorsal,
                Time = p.Time,
                Banned = p.Banned,
                User = new UserDto(
                    p.User.Id,
                    p.User.Email,
                    p.User.Name,
                    p.User.Roles,
                    p.User.Age,
                    p.User.Gender,
                    p.User.Image,
                    p.User.Banned
                ),
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
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ParticipantDto?> RegisterAsync(CreateParticipantDto dto)
    {
        var race = await _context.TrailRunnings
            .Include(r => r.TrailRunningParticipants)
            .FirstOrDefaultAsync(r => r.Id == dto.TrailRunning);

        if (race == null) return null;

        var user = await _context.Users.FindAsync(dto.User);
        if (user == null) return null;

        var existingDorsals = race.TrailRunningParticipants.Select(p => p.Dorsal).ToHashSet();
        
        var rand = new Random();
        int dorsal;
        do
        {
            dorsal = rand.Next(1, race.AvailableSlots * 2 + 1);
        } while (existingDorsals.Contains(dorsal));

        var participant = new TrailRunningParticipant
        {
            UserId = dto.User,
            TrailRunningId = dto.TrailRunning,
            Dorsal = dorsal,
            Time = "0",
            Banned = false
        };

        _context.TrailRunningParticipants.Add(participant);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(participant.Id);
    }

    public async Task<ParticipantDto?> UpdateAsync(int id, UpdateParticipantDto dto)
    {
        var participant = await _context.TrailRunningParticipants.FindAsync(id);
        if (participant == null) return null;

        if (dto.Dorsal.HasValue) participant.Dorsal = dto.Dorsal.Value;
        if (dto.Time != null) participant.Time = dto.Time;
        if (dto.Banned.HasValue) participant.Banned = dto.Banned.Value;

        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await _context.TrailRunningParticipants
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
            
        return deleted > 0;
    }
}
