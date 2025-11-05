using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Events.Commands;

public class UpdateEventHandler(IEventRepository eventRepository, IMapper mapper) 
: IRequestHandler<UpdateEventCommand, Unit>
{
    public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
      var existingEvent = await eventRepository.GetEventByIdAsync(request.EventId, cancellationToken);
        
        if (existingEvent == null)
        {
            throw new Exception("Event not found."); 
        }

        if (existingEvent.HostId != request.OrganizerId)
        {
            throw new UnauthorizedAccessException("Only the event organizer can modify the event.");
        }

        mapper.Map(request.updateEventDto, existingEvent); 
        
        await eventRepository.UpdateEventAsync(existingEvent, cancellationToken); 
        
        return Unit.Value;
    }
}

