namespace Domain;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Username { get; set; }

    public ICollection<Event> HostedEvents { get; set; } = [];
    public ICollection<EventParticipant> Participations { get; set; } = [];

}