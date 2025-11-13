using Application.Events.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Events.Queries;

public class GetSingleEventHandler(IEventRepository eventRepository, IMapper mapper) 
: IRequestHandler<GetSingleEventQuery, EventDetailDto>
{
    public async Task<EventDetailDto> Handle(GetSingleEventQuery request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken);

        if (@event is null)
        {
            throw new Exception("Event not found"); 
        }
        
        var eventDto = mapper.Map<EventDetailDto>(@event);

        if (request.UserId.HasValue)
        {
            var userId = request.UserId.Value;
            eventDto.IsOrganizer = @event.HostId == userId;
            eventDto.IsJoined = @event.Participants.Any(ue => ue.UserId == userId);
        }
        
        eventDto.ParticipantsCount = @event.Participants.Count;
        return eventDto;
    }
}