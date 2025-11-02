using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Events.Commands;

public class DeleteEventHandler(IEventRepository eventRepository) : IRequestHandler<DeleteEventCommand, Unit>
{
    public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetEventByIdAsync(request.id, cancellationToken);

        if (@event is null)
        {
            throw new Exception("Event not found");
        }

        await eventRepository.DeleteEventAsync(@event, cancellationToken);

        return Unit.Value;
    }
}