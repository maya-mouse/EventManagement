namespace Application.Events.DTOs;

public class CreateEventDto
{
    public required string Title { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public required DateTime DateTime { get; set; }
    public required string Location { get; set; } = string.Empty;
    public int? Capacity { get; set; } 
    public bool IsPublic { get; set; }
}
