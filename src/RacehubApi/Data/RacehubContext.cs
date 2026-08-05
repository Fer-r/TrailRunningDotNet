using Microsoft.EntityFrameworkCore;
using RacehubApi.Models;

namespace RacehubApi.Data;

public class RacehubContext(DbContextOptions<RacehubContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<TrailRunning> TrailRunnings { get; set; } = null!;
    public DbSet<TrailRunningParticipant> TrailRunningParticipants { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unique email constraint for User
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
            
        // Configure relationships
        modelBuilder.Entity<TrailRunningParticipant>()
            .HasOne(p => p.User)
            .WithMany(u => u.TrailRunningParticipants)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TrailRunningParticipant>()
            .HasOne(p => p.TrailRunning)
            .WithMany(t => t.TrailRunningParticipants)
            .HasForeignKey(p => p.TrailRunningId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
