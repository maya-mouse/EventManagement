namespace Domain;

public class EventParticipant
{
    public int EventId { get; set; }
    public int UserId { get; set; }

    public Event Event { get; set; } = null!;
    public User User { get; set; } = null!;

    public DateTime JoinDate { get; set; } = DateTime.UtcNow;
}