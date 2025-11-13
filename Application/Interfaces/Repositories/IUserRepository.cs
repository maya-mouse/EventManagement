using Domain;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken);
    Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken);
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
}