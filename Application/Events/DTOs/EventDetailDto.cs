namespace Application.Events.DTOs;

public class EventDetailDto
{
    public int Id { get; set; }
    public required string Title { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public required string Location { get; set; } = string.Empty;
    public int? Capacity { get; set; }
    public bool IsPublic { get; set; }

    public ParticipantDto Host { get; set; } = null!;
    public int HostId { get; set; }

    public List<ParticipantDto> Participants { get; set; } = new List<ParticipantDto>();

    public bool IsJoined { get; set; }
    public bool IsOrganizer { get; set; }
    
    public int ParticipantsCount { get; set; }
    public bool IsFull => Capacity.HasValue && ParticipantsCount >= Capacity.Value;
}