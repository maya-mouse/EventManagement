using System.ComponentModel.DataAnnotations;

namespace Application.Users.DTOs;

public class RegisterDto
{
    public required string Username { get; set; } = "";
    [EmailAddress]
    public required string Email { get; set; } = "";
    public required string Password { get; set; } = "";
    [Compare(nameof(Password))]
    public required string ConfirmPassword { get; set; } = "";
}