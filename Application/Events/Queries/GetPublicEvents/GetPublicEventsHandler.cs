using Application.Events.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Events.Queries;

public class GetPublicEventsHandler(IEventRepository eventRepository, IMapper mapper) : IRequestHandler<GetPublicEventsQuery, List<EventDto>>
{
    public async Task<List<EventDto>> Handle(GetPublicEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await eventRepository.GetPublicEventsAsync(cancellationToken); 

        var eventDtos = mapper.Map<List<EventDto>>(events);      
            
     return eventDtos;
    }

}