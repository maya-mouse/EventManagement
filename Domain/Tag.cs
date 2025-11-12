namespace Domain;

public class Tag
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<EventTag> EventTags { get; set; } = [];
}