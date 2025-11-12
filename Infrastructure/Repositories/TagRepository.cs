using Application.Interfaces.Repositories;
using Domain;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppDbContext _context;
    public TagRepository(AppDbContext context) => _context = context;

    public async Task<Tag> AddTagAsync(Tag newTag, CancellationToken cancellationToken)
    {
        _context.Tags.Add(newTag);
        await _context.SaveChangesAsync(cancellationToken);
        return newTag;
    }

    public async Task<List<Tag>> GetAllTagsAsync(CancellationToken cancellationToken)
    {
        return await _context.Tags.ToListAsync(cancellationToken);
    }

   public async Task<List<Tag>> GetTagsByNamesAsync(List<string> names, CancellationToken cancellationToken)
    {
        var normalizedNames = names.Select(n => n.ToLower()).ToList();

        return await _context.Tags
            .Where(t => normalizedNames.Contains(t.Name.ToLower()))
            .ToListAsync(cancellationToken);
    }
}