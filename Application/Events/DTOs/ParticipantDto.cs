namespace Application.Events.DTOs;

public class ParticipantDto
{
    public int UserId { get; set; }
    public required string Username { get; set; } = "";
    public required string Email { get; set; } = "";
}