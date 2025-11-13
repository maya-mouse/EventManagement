using Application.Interfaces.Repositories;
using Application.Tags.DTOs;
using AutoMapper;
using MediatR;

public class GetAllTagsQueryHandler(ITagRepository tagRepository, IMapper mapper)
: IRequestHandler<GetAllTagsQuery, List<TagDto>>
{
    public async Task<List<TagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await tagRepository.GetAllTagsAsync(cancellationToken);

        var tagsDto = mapper.Map<List<TagDto>>(tags);

        return tagsDto;
    }
}