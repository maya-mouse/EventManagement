namespace Application.Events.DTOs;

public class CalendarEventDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public DateTime DateTime { get; set; } 
        public bool IsOrganizer { get; set; }  
    }