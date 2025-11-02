using Application.Events.DTOs;
using MediatR;

namespace Application.Events.Commands;

public record UpdateEventCommand(CreateEventCommand updadtEventDto) : IRequest<Unit>;