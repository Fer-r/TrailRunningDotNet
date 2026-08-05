using Microsoft.EntityFrameworkCore;
using RacehubApi.Data;
using RacehubApi.Models;
using RacehubApi.Services;
using Xunit;

namespace RacehubApi.Tests;

public class UnitTests : IDisposable
{
    private readonly RacehubContext _context;
    private readonly ParticipantService _participantService;

    public UnitTests()
    {
        var options = new DbContextOptionsBuilder<RacehubContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        _context = new RacehubContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();

        _participantService = new ParticipantService(_context);
    }

    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }

    [Fact]
    public async Task RegisterAsync_AssignsUniqueDorsal()
    {
        // Arrange
        var user1 = new User { Id = 1, Name = "User 1", Email = "user1@test.com", Password = "123", Roles = "[]" };
        var user2 = new User { Id = 2, Name = "User 2", Email = "user2@test.com", Password = "123", Roles = "[]" };
        var race = new TrailRunning { Id = 1, Name = "Race 1", AvailableSlots = 10, Date = DateTime.Now, Description = "", DistanceKm = 10, EntryFee = 10, Location = "Madrid", Coordinates = "0,0", Category = "General", Status = "Open" };
        
        _context.Users.AddRange(user1, user2);
        _context.TrailRunnings.Add(race);
        await _context.SaveChangesAsync();

        // Act
        var result1 = await _participantService.RegisterAsync(new RacehubApi.DTOs.CreateParticipantDto(race.Id, user1.Id));
        var result2 = await _participantService.RegisterAsync(new RacehubApi.DTOs.CreateParticipantDto(race.Id, user2.Id));

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.NotEqual(result1.Dorsal, result2.Dorsal);
        Assert.True(result1.Dorsal >= 1 && result1.Dorsal <= race.AvailableSlots * 2);
    }

    [Fact]
    public void VerifyPassword_CorrectPassword_ReturnsTrue()
    {
        var plain = "mySecretPassword123";
        var hash = BCrypt.Net.BCrypt.HashPassword(plain);

        var isValid = BCrypt.Net.BCrypt.Verify(plain, hash);

        Assert.True(isValid);
    }

    [Fact]
    public void VerifyPassword_WrongPassword_ReturnsFalse()
    {
        var plain = "mySecretPassword123";
        var hash = BCrypt.Net.BCrypt.HashPassword(plain);

        var isValid = BCrypt.Net.BCrypt.Verify("wrongPassword", hash);

        Assert.False(isValid);
    }
}
