using Application.Interfaces.Repositories;
using Application.Events.DTOs;
using MediatR;

namespace Application.Events.Queries;

public class GetUserEventsHandler(IEventRepository eventRepository)
: IRequestHandler<GetUserEventsQuery, List<CalendarEventDto>>
{
    public async Task<List<CalendarEventDto>> Handle(GetUserEventsQuery request, CancellationToken cancellationToken)
    {

        var joinedEvents = await eventRepository.GetUserEventsAsync(request.UserId, cancellationToken);

        var organizedEvents = await eventRepository.GetUserEventsAsync(request.UserId, cancellationToken); // ПОТРІБЕН НОВИЙ МЕТОД РЕПОЗИТОРІЮ

        var allEvents = joinedEvents.Union(organizedEvents).ToList();

        var dtos = allEvents.Select(e => new CalendarEventDto
        {
            Id = e.Id,
            Title = e.Title,
            DateTime = e.DateTime,
            IsOrganizer = e.HostId == request.UserId
        }).ToList();

        return dtos;
    }
}