namespace Application.Events.DTOs;

public class CreateEventDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public int? Capacity { get; set; } 
    public bool IsPublic { get; set; }
}
