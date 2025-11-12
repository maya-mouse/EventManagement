using Application.Interfaces.Repositories;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Events.Commands;

public class CreateEventHandler(IEventRepository eventRepository,
ITagRepository tagRepository, IMapper mapper) 
: IRequestHandler<CreateEventCommand, int>
{
    public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var newEvent = mapper.Map<Event>(request.createEventDto);

        newEvent.HostId = request.hostId;

        if (request.createEventDto.TagNames != null && request.createEventDto.TagNames.Any())
        {
            var uniqueTagNames = request.createEventDto.TagNames.Distinct().ToList();

            var existingTags = await tagRepository.GetTagsByNamesAsync(uniqueTagNames, cancellationToken); // Потрібен новий метод

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
            
            newEvent.EventTags = tagsToAdd.Select(t => new EventTag { Event = newEvent, Tag = t }).ToList();
        }

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