using System.ComponentModel.DataAnnotations;

namespace Application.Users.DTOs;

public class AuthResponseDto
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string Token { get; set; }
}