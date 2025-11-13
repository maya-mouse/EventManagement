using Application.Events.DTOs;
using Domain;
using MediatR;

namespace Application.Events.Queries;

public record GetPublicEventsQuery(int? UserId, string? SearchTerm,
List<string>? TagNames) : IRequest<List<EventDto>>;