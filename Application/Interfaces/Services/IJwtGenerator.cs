namespace Application.Interfaces.Services;

public interface IJwtGenerator
{
    public string GenerateToken(int userId, string email, string username);
}