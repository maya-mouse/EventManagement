using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Events.Commands;

public class LeaveEventHandler(IEventRepository eventRepository) : IRequestHandler<LeaveEventCommand, Unit>
{
    public async Task<Unit> Handle(LeaveEventCommand request, CancellationToken cancellationToken)
    {
        await eventRepository.LeaveEventAsync(request.eventId, request.userId, cancellationToken);
        return Unit.Value;
    }
}