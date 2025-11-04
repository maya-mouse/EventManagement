using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Users.DTOs;
using MediatR;

namespace Application.Users.Commands.Login;

public class LoginHandler (IUserRepository userRepository,
IPasswordHasher passwordHasher, IJwtGenerator jwtGenerator): IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Incorrect email or password"); 
        }

        if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
             throw new UnauthorizedAccessException("Incorrect email or password");
        }

        var token = jwtGenerator.GenerateToken(user.Id, user.Email, user.Username);

        return new AuthResponseDto 
        { 
            Email = user.Email, 
            Username = user.Username,
            Token = token 
        };
    }
}