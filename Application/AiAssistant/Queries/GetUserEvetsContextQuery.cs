using MediatR;

namespace Application.AiAssistant.Queries;

public record GetUserEventsContextQuery(int UserId) : IRequest<string>;
