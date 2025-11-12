using Application.Events.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;

using MediatR;

namespace Application.Events.Queries;

public class GetPublicEventsHandler(IEventRepository eventRepository, IMapper mapper) : IRequestHandler<GetPublicEventsQuery, List<EventDto>>
{
    public async Task<List<EventDto>> Handle(GetPublicEventsQuery request, CancellationToken cancellationToken)
    {

        var events = await eventRepository.GetPublicEventsAsync(
            request.SearchTerm,
            request.TagNames,
            cancellationToken); 
        var eventDtos = mapper.Map<List<EventDto>>(events);

        if (request.UserId.HasValue)
        {
            var userId = request.UserId.Value;

            foreach (var dto in eventDtos)
            {
               
                var eventEntity = events.First(e => e.Id == dto.Id); 
                dto.IsJoined = eventEntity.Participants.Any(ue => ue.UserId == userId);
            }
        }    
        return eventDtos;
    }

}