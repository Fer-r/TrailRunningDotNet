using System.ComponentModel.DataAnnotations;

namespace RacehubApi.Models;

public class TrailRunning
{
    public int Id { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    public string Description { get; set; } = "";
    
    public DateTime Date { get; set; }
    
    public int DistanceKm { get; set; }
    
    [Required]
    public required string Location { get; set; }
    
    public string? Coordinates { get; set; }
    
    public int? Unevenness { get; set; }
    
    public int EntryFee { get; set; }
    
    public int AvailableSlots { get; set; }
    
    public string Status { get; set; } = "open";
    
    public string? Category { get; set; }
    
    public string? Image { get; set; }
    
    public string? Gender { get; set; }

    public List<TrailRunningParticipant> TrailRunningParticipants { get; set; } = [];
}
