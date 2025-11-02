using MediatR;

namespace Application.Events.Commands;

public record LeaveEventCommand(int eventId, int userId) : IRequest<Unit>;