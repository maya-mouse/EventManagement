using Domain;

namespace Application.Interfaces.Repositories;

public interface IEventRepository
{
        Task<List<Event>> GetPublicEventsAsync(string? searchTerm,
        List<string>? tagNames, CancellationToken cancellationToken);
        Task<Event?> GetEventByIdAsync(int eventId, CancellationToken cancellationToken);
        Task<List<Event>> GetUserEventsAsync(int userId, CancellationToken cancellationToken);
        Task<Event> AddEventAsync(Event newEvent, CancellationToken cancellationToken);
        Task UpdateEventAsync(Event eventToUpdate, CancellationToken cancellationToken);
        Task DeleteEventAsync(Event eventToDelete, CancellationToken cancellationToken);
        Task<bool> IsUserJoinedAsync(int userId, int eventId, CancellationToken cancellationToken);
        Task JoinEventAsync(EventParticipant userEvent, CancellationToken cancellationToken);
        Task LeaveEventAsync(int userId, int eventId, CancellationToken cancellationToken);
        Task RemoveAllTagsFromEventAsync(int eventId, CancellationToken cancellationToken);
        Task<List<Event>> GetAllUserEventsForAIAsync(int userId, CancellationToken cancellationToken);
}