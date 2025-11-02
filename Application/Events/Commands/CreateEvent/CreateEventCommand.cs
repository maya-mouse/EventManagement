using Application.Events.DTOs;
using MediatR;

namespace Application.Events.Commands;

public record CreateEventCommand(CreateEventDto createEventDto, int hostId) : IRequest<int>;