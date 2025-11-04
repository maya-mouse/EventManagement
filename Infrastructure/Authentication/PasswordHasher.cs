using Application.Interfaces.Services;
using static BCrypt.Net.BCrypt;

namespace Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return Verify(password, hash);
    }
}