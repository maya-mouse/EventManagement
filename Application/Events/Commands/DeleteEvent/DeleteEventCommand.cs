using MediatR;

namespace Application.Events.Commands;

public record DeleteEventCommand(int id) : IRequest<Unit>;