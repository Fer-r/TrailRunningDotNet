namespace RacehubApi.Models;

public class TrailRunningParticipant
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int TrailRunningId { get; set; }
    public TrailRunning TrailRunning { get; set; } = null!;
    
    public int Dorsal { get; set; }
    public string? Time { get; set; }
    public bool Banned { get; set; }
}
