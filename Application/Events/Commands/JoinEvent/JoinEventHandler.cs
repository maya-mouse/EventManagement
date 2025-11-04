using Application.Interfaces.Repositories;
using Domain;
using MediatR;

namespace Application.Events.Commands;
public class JoinEventCommandHandler : IRequestHandler<JoinEventCommand, Unit>
{
    private readonly IEventRepository _eventRepository;

    public JoinEventCommandHandler(IEventRepository eventRepository) => _eventRepository = eventRepository;
    public async Task<Unit> Handle(JoinEventCommand request, CancellationToken cancellationToken)
    {
        
        var eventExists = await _eventRepository.GetEventByIdAsync(request.EventId, cancellationToken);
        if (eventExists == null) 
        {
            throw new Exception("Event not found."); 
        }

      
        if (await _eventRepository.IsUserJoinedAsync(request.UserId, request.EventId, cancellationToken))
        {
            throw new Exception("User already joined this event.");
        }

        var userEvent = new EventParticipant
        {
            UserId = request.UserId,
            EventId = request.EventId
        };

        await _eventRepository.JoinEventAsync(userEvent, cancellationToken);

        return Unit.Value;
    }
}