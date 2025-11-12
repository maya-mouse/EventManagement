using Application.Interfaces.Repositories;
using Domain;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;
    public EventRepository(AppDbContext context) => _context = context;
    public async Task<List<Event>> GetPublicEventsAsync(
        string? searchTerm,
        List<string>? tagNames,
        CancellationToken cancellationToken)
    {
        var query = _context.Events
        .Where(e => e.IsPublic && e.DateTime > DateTime.UtcNow)
        .Include(i => i.Participants)
        .Include(et => et.EventTags)
        .ThenInclude(t => t.Tag)
        .AsSplitQuery()
        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(e =>
                e.Title.Contains(searchTerm) ||
                e.Description.Contains(searchTerm) ||
                e.Location.Contains(searchTerm));
        }

        if (tagNames != null && tagNames.Count > 0)
        {       
            query = query.Where(e => tagNames.All(tagName => 
        e.EventTags.Any(et => et.Tag.Name.ToLower() == tagName.ToLower())));
           
        }
        return await query
            .OrderBy(e => e.DateTime)
            .ToListAsync(cancellationToken);

        } 
            

    public async Task<Event?> GetEventByIdAsync(int eventId, CancellationToken cancellationToken) =>
           await _context.Events.Include(e => e.Host)
           .Include(e => e.Participants)
           .ThenInclude(u => u.User)
           .Include(et => et.EventTags)
           .ThenInclude(t => t.Tag)
           .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

    public async Task<List<Event>> GetUserEventsAsync(int userId, CancellationToken cancellationToken) =>
        await _context.Events
        .Where(e => e.Participants.Any(ep => ep.UserId == userId)) 
        .Include(e => e.EventTags)
            .ThenInclude(et => et.Tag)
            .Include(e => e.Host)
        .ToListAsync(cancellationToken);

    public async Task<Event> AddEventAsync(Event newEvent, CancellationToken cancellationToken)
    {
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync(cancellationToken);
        return newEvent;
    }

    public async Task UpdateEventAsync(Event editEvent, CancellationToken cancellationToken)
    {
        _context.Events.Update(editEvent);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteEventAsync(Event @event, CancellationToken cancellationToken)
    {
        _context.Events.Remove(@event);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsUserJoinedAsync(int userId, int eventId, CancellationToken cancellationToken) =>
            await _context.EventParticipants.AnyAsync(ue => ue.UserId == userId && ue.EventId == eventId, cancellationToken);

    public async Task JoinEventAsync(EventParticipant userEvent, CancellationToken cancellationToken)
    {
        _context.EventParticipants.Add(userEvent);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task LeaveEventAsync(int userId, int eventId, CancellationToken cancellationToken)
    {
        var userEvent = await _context.EventParticipants
          .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EventId == eventId, cancellationToken);

        if (userEvent != null)
        {
            _context.EventParticipants.Remove(userEvent);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RemoveAllTagsFromEventAsync(int eventId, CancellationToken cancellationToken)
    {
        await _context.EventTags
            .Where(et => et.EventId == eventId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}