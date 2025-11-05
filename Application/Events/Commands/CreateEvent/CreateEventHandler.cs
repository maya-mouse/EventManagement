using Application.Interfaces.Repositories;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Events.Commands;

public class CreateEventHandler(IEventRepository eventRepository, IMapper mapper) 
: IRequestHandler<CreateEventCommand, int>
{
    public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
            var newEvent = mapper.Map<Event>(request.createEventDto);

            newEvent.HostId = request.hostId;

            var createdEvent = await eventRepository.AddEventAsync(newEvent, cancellationToken);

            var userEvent = new EventParticipant
            {
                UserId = request.hostId,
                EventId = createdEvent.Id,
                JoinDate = DateTime.UtcNow
            };

            await eventRepository.JoinEventAsync(userEvent, cancellationToken);
           
            return createdEvent.Id;
    }
}