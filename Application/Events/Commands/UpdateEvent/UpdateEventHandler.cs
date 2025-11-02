using Application.Interfaces.Repositories;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Events.Commands;

public class UpdateEventHandler(IEventRepository eventRepository, IMapper mapper) 
: IRequestHandler<UpdateEventCommand, Unit>
{
    public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var newEvent = mapper.Map<Event>(request);

        await eventRepository.UpdateEventAsync(newEvent, cancellationToken);
           
        return Unit.Value;
    }
}

