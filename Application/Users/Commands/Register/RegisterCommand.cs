
using Application.Users.DTOs;
using MediatR;

namespace Application.Users.Commands.Register;

public record RegisterCommand(string Email, string Username,
string Password, string ConfirmPassword) : IRequest<AuthResponseDto>;