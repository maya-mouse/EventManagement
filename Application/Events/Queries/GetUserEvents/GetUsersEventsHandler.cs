using Application.Events.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Events.Queries;


public class GetUserEventsQueryHandler(IEventRepository eventRepository, IMapper mapper) 
: IRequestHandler<GetUserEventsQuery, List<EventDto>>
{

    public async Task<List<EventDto>> Handle(GetUserEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await eventRepository.GetUserEventsAsync(request.UserId, cancellationToken);
        
        var eventDtos = mapper.Map<List<EventDto>>(events);
        
        return eventDtos; 
    }
}