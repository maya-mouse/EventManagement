using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Users.DTOs;
using Domain;
using MediatR;

namespace Application.Users.Commands.Register;

public class RegisterHandler(IUserRepository userRepository,
IPasswordHasher passwordHasher, IJwtGenerator jwtGenerator) : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.IsEmailExistsAsync(request.Email, cancellationToken))
        {
            throw new Exception("This user exists");
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHasher.HashPassword(request.Password)
        };

        var result = await userRepository.AddUserAsync(user, cancellationToken);

        var token = jwtGenerator.GenerateToken(result.Id, result.Email);

        return new AuthResponseDto { Email = user.Email, Token = token };
    }
}