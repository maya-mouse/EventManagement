using MediatR;

namespace Application.Events.Commands;

public record JoinEventCommand(int EventId, int UserId) : IRequest<Unit>;

