using Application.Events.DTOs;
using MediatR;

namespace Application.Events.Queries;
public record GetUserEventsQuery(int UserId) : IRequest<List<EventDto>>;

