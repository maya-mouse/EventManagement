using Application.Events.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Events.Queries;

public class GetSingleEventHandler(IEventRepository eventRepository, IMapper mapper) 
: IRequestHandler<GetSingleEventQuery, EventDto>
{
    public async Task<EventDto> Handle(GetSingleEventQuery request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetEventByIdAsync(request.eventId, cancellationToken);

        if (@event is null)
        {
            throw new Exception("Event not found");
        }

        var eventDto = mapper.Map<EventDto>(@event);

        return eventDto;
    }
}