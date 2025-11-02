using Application.Users.DTOs;
using MediatR;

namespace Application.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;