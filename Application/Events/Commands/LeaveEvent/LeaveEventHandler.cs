using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Events.Commands;

public class LeaveEventHandler(IEventRepository eventRepository) : IRequestHandler<LeaveEventCommand, Unit>
{
    public async Task<Unit> Handle(LeaveEventCommand request, CancellationToken cancellationToken)
    {
        await eventRepository.LeaveEventAsync(request.userId, request.eventId, cancellationToken);
        return Unit.Value;
    }
}