using MediatR;

namespace Application.Tags.DTOs;
public record GetAllTagsQuery() : IRequest<List<TagDto>>;