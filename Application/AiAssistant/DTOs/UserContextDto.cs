namespace Application.AiAssistant.DTOs;

public class UserContextDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<EventAiDto> Events { get; set; } = new();
    }