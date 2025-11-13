using System.Text.Json;
using Application.AiAssistant.DTOs;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.AiAssistant.Queries;

public class GetUserEventsContextHandler(IEventRepository eventRepository, IUserRepository userRepository) 
        : IRequestHandler<GetUserEventsContextQuery, string>
    {
        public async Task<string> Handle(GetUserEventsContextQuery request, CancellationToken cancellationToken)
        {

            var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
            
            var events = await eventRepository.GetAllUserEventsForAIAsync(request.UserId, cancellationToken);
            
            var contextDto = new UserContextDto 
            {
                UserId = request.UserId,
                Username = user?.Username ?? $"User-{request.UserId}",
                Events = events.Select(e => new EventAiDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    DateTime = e.DateTime,
                    Location = e.Location,
                    IsOrganizer = e.HostId == request.UserId,
                    
                    Tags = e.EventTags
                        .Select(et => et.Tag.Name)
                        .ToList(),
                        
                    Participants = e.Participants
                        .Where(p => p.User != null)
                        .Select(p => p.User.Username)
                        .ToList() 
                }).ToList()
            };

            var jsonOptions = new JsonSerializerOptions { WriteIndented = false, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
            
            return JsonSerializer.Serialize(contextDto, jsonOptions);
        }
    }