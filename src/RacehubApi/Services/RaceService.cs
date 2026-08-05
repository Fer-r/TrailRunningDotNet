using Microsoft.EntityFrameworkCore;
using RacehubApi.Data;
using RacehubApi.DTOs;
using RacehubApi.Models;

namespace RacehubApi.Services;

public class RaceService(RacehubContext context)
{
    public async Task<IReadOnlyList<RaceDto>> GetAllAsync()
    {
        return await context.TrailRunnings
            .AsNoTracking()
            .Select(r => new RaceDto(
                r.Id,
                r.Name,
                r.Description,
                r.Date,
                r.DistanceKm,
                r.Location,
                r.Coordinates,
                r.Unevenness,
                r.EntryFee,
                r.AvailableSlots,
                r.Status,
                r.Category,
                r.Image,
                r.Gender,
                r.TrailRunningParticipants.Select(p => new RaceParticipantDto(
                    p.Id,
                    p.UserId,
                    p.TrailRunningId,
                    p.Dorsal,
                    p.Time,
                    p.Banned,
                    new UserSimpleDto(p.User.Id, p.User.Name)
                )).ToList()
            ))
            .ToListAsync();
    }

    public async Task<RaceDto?> GetByIdAsync(int id)
    {
        return await context.TrailRunnings
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new RaceDto(
                r.Id,
                r.Name,
                r.Description,
                r.Date,
                r.DistanceKm,
                r.Location,
                r.Coordinates,
                r.Unevenness,
                r.EntryFee,
                r.AvailableSlots,
                r.Status,
                r.Category,
                r.Image,
                r.Gender,
                r.TrailRunningParticipants.Select(p => new RaceParticipantDto(
                    p.Id,
                    p.UserId,
                    p.TrailRunningId,
                    p.Dorsal,
                    p.Time,
                    p.Banned,
                    new UserSimpleDto(p.User.Id, p.User.Name)
                )).ToList()
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<RaceDto> CreateAsync(RaceCreateDto dto)
    {
        var race = new TrailRunning
        {
            Name = dto.Name,
            Description = dto.Description ?? "",
            Date = dto.Date,
            DistanceKm = dto.DistanceKm,
            Location = dto.Location,
            Coordinates = dto.Coordinates,
            Unevenness = dto.Unevenness,
            EntryFee = dto.EntryFee,
            AvailableSlots = dto.AvailableSlots,
            Status = dto.Status ?? "open",
            Category = dto.Category,
            Image = dto.Image,
            Gender = dto.Gender
        };

        context.TrailRunnings.Add(race);
        await context.SaveChangesAsync();

        return await GetByIdAsync(race.Id) ?? throw new InvalidOperationException("Failed to retrieve created race");
    }

    public async Task<RaceDto?> UpdateAsync(int id, RaceUpdateDto dto)
    {
        var race = await context.TrailRunnings.FindAsync(id);
        if (race == null) return null;

        if (dto.Name != null) race.Name = dto.Name;
        if (dto.Description != null) race.Description = dto.Description;
        if (dto.Date.HasValue) race.Date = dto.Date.Value;
        if (dto.DistanceKm.HasValue) race.DistanceKm = dto.DistanceKm.Value;
        if (dto.Location != null) race.Location = dto.Location;
        if (dto.Coordinates != null) race.Coordinates = dto.Coordinates;
        if (dto.Unevenness.HasValue) race.Unevenness = dto.Unevenness.Value;
        if (dto.EntryFee.HasValue) race.EntryFee = dto.EntryFee.Value;
        if (dto.AvailableSlots.HasValue) race.AvailableSlots = dto.AvailableSlots.Value;
        if (dto.Status != null) race.Status = dto.Status;
        if (dto.Category != null) race.Category = dto.Category;
        if (dto.Image != null) race.Image = dto.Image;
        if (dto.Gender != null) race.Gender = dto.Gender;

        await context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var exists = await context.TrailRunnings.AnyAsync(r => r.Id == id);
        if (!exists) return false;

        // Cascada manual usando ExecuteDeleteAsync() como se especificó en el plan
        await context.TrailRunningParticipants
            .Where(p => p.TrailRunningId == id)
            .ExecuteDeleteAsync();

        await context.TrailRunnings
            .Where(r => r.Id == id)
            .ExecuteDeleteAsync();

        return true;
    }
}
