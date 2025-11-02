using Application.Events.DTOs;
using MediatR;

namespace Application.Events.Queries;

public record GetSingleEventQuery(int eventId) : IRequest<EventDto>;