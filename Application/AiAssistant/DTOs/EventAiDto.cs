namespace Application.AiAssistant.DTOs;

public class EventAiDto
    {
        public int Id { get; set; }
        public required string Title { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public required string Location { get; set; } = string.Empty;
        public bool IsOrganizer { get; set; }
        public List<string> Tags { get; set; } = new();
        public List<string> Participants { get; set; } = new(); 
    }