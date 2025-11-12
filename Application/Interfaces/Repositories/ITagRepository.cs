using Domain;

namespace Application.Interfaces.Repositories;

public interface ITagRepository
{
    Task<List<Tag>> GetAllTagsAsync(CancellationToken cancellationToken);
    Task<List<Tag>> GetTagsByNamesAsync(List<string> names, CancellationToken cancellationToken);
    Task<Tag> AddTagAsync(Tag tag, CancellationToken cancellationToken);
}