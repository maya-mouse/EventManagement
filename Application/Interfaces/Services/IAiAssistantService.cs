namespace Application.Interfaces.Services;

public interface IAiAssistantService
{
    Task<string> GetAnswerAsync(string useQuery, string contextData, CancellationToken cancellationToken);
}