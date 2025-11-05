
namespace Domain;
public class Event
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime DateTime { get; set; }
    public required string Location { get; set; }
    public int? Capacity { get; set; }
    public bool IsPublic { get; set; }

    public int HostId { get; set; }
    public User Host { get; set; } = null!;

    public ICollection<EventParticipant> Participants { get; set; } = [];

}