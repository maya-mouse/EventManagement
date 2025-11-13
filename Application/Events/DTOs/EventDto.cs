using Application.Tags.DTOs;

namespace Application.Events.DTOs;

public class EventDto
{
    public int Id { get; set; }
    public required string Title { get; set; } = "";
    public required string Description { get; set; } = "";
    public DateTime DateTime { get; set; }
    public required string Location { get; set; } = "";
    public int? Capacity { get; set; }
    public int ParticipantsCount { get; set; }
    public bool IsFull => Capacity.HasValue && ParticipantsCount >= Capacity.Value;

    public bool IsJoined { get; set; }
    public List<TagDto> Tags { get; set; } = new List<TagDto>();


}