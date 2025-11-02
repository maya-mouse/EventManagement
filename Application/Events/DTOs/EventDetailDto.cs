namespace Application.Events.DTOs;
public class EventDetailDto
{
    public int Id { get; set; }
    public required string Title { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty; // Повний опис
    public DateTime DateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public int? Capacity { get; set; }
    public bool IsPublic { get; set; }

    public ParticipantDto Host { get; set; } = null!;
    public int HostId { get; set; }


    public List<ParticipantDto> Participants { get; set; } = new List<ParticipantDto>();


    public bool IsJoined { get; set; }
    public bool IsOrganizer { get; set; }
}