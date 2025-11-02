using Application.Events.DTOs;
using Domain;
using MediatR;

namespace Application.Events.Queries;

public record GetPublicEventsQuery() : IRequest<List<EventDto>>;