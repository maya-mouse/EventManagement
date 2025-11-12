using Application.Interfaces.Repositories;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Events.Commands;

public class UpdateEventHandler(IEventRepository eventRepository,
ITagRepository tagRepository, IMapper mapper) 
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

        if (request.updateEventDto.TagNames != null && request.updateEventDto.TagNames.Any())
        {
            var uniqueTagNames = request.updateEventDto.TagNames.ToList();

            var existingTags = await tagRepository.GetTagsByNamesAsync(uniqueTagNames, cancellationToken);

            var tagsToAdd = new List<Tag>();

            foreach (var tagName in uniqueTagNames)
            {
                var existingTag = existingTags.FirstOrDefault(t => t.Name.ToLower() == tagName.ToLower());

                if (existingTag != null)
                {
                    tagsToAdd.Add(existingTag);
                }
                else
                {
                    var newTag = new Tag { Name = tagName };
                    await tagRepository.AddTagAsync(newTag, cancellationToken);
                    tagsToAdd.Add(newTag);
                }   
            }

            existingEvent.EventTags = tagsToAdd.Select(t => new EventTag { Event = existingEvent, Tag = t }).ToList();
        }
        await eventRepository.UpdateEventAsync(existingEvent, cancellationToken); 
        
        return Unit.Value;
    }
}

