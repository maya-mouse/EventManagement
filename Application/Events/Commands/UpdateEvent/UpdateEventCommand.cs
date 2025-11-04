using Application.Events.DTOs;
using MediatR;

namespace Application.Events.Commands;

public record UpdateEventCommand(int EventId, int OrganizerId,CreateEventDto updateEventDto) : IRequest<Unit>;