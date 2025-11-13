using Application.Interfaces.Services;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContextSeeder
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher; 

    public AppDbContextSeeder(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {

        if (await _context.Users.AnyAsync())
        {
            Console.WriteLine("INFO: Users found. Skipping seeding.");
            return;
        }

        var alicePassword = "pass123";
        var aliceHash = _passwordHasher.HashPassword(alicePassword);
        var bobPassword = "123pass";
        var bobHash = _passwordHasher.HashPassword(bobPassword);

        var alice = new User { Email = "alice.org@event.com", PasswordHash = aliceHash, Username = "Alice" };
        var bob = new User { Email = "bob4321@event.com", PasswordHash = bobHash, Username = "Bob" };

        _context.Users.AddRange(alice, bob);
        await _context.SaveChangesAsync();

        var tags = new List<Tag>
        {
            new Tag
            {
                Name = "Tech"
            },
            new Tag
            {
                Name = "Art"
            },
            new Tag
            {
                Name = "Business"
            },
            new Tag
            {
                Name = "Music"
            },
            new Tag
            {
                Name = "Entertainment"
            }
        };

        _context.Tags.AddRange(tags);
        await _context.SaveChangesAsync();


        var events = new List<Event>
        {    new Event
            {
                HostId = alice.Id,
                Title = "Annual Tech Conference",
                Description = "Largest tech gathering of the year.",
                DateTime = DateTime.UtcNow.AddDays(30),
                Location = "Kyiv Expo Center",
                Capacity = 100,
                IsPublic = true,
            },
            new Event
            {
                HostId = alice.Id,
                Title = "Open Source Workshop",
                Description = "Hands-on coding session for contributors.",
                DateTime = DateTime.UtcNow.AddDays(45),
                Location = "Online via Zoom",
                Capacity = 50,
                IsPublic = true
            },
            new Event
            {
                HostId = bob.Id,
                Title = "Local Book Club Meeting",
                Description = "Discussion on classic literature.",
                DateTime = DateTime.UtcNow.AddDays(15),
                Location = "Central Library",
                Capacity = null,
                IsPublic = true
            },
            new Event
            {
                HostId = alice.Id,
                Title = "Private Team Dinner",
                Description = "Team building event (Private)",
                DateTime = DateTime.UtcNow.AddDays(10),
                Location = "Italian Restaurant",
                Capacity = 10,
                IsPublic = false
            }
        };

        _context.Events.AddRange(events);
        await _context.SaveChangesAsync(); 

        var eventTags = new List<EventTag>
    {
        new EventTag { EventId = events[0].Id, TagId = tags[0].Id },
        new EventTag { EventId = events[1].Id, TagId = tags[0].Id},
        new EventTag { EventId = events[1].Id, TagId = tags[2].Id},
        new EventTag { EventId = events[2].Id, TagId = tags[0].Id },
        new EventTag { EventId = events[2].Id, TagId = tags[1].Id },
        new EventTag { EventId = events[2].Id, TagId = tags[4].Id },
        new EventTag { EventId = events[3].Id, TagId = tags[4].Id},
    };

    _context.EventTags.AddRange(eventTags);
    await _context.SaveChangesAsync();

    var eventParticipants = new List<EventParticipant>
    {
        new EventParticipant { EventId = events[0].Id, UserId = alice.Id, JoinDate = DateTime.UtcNow },
        new EventParticipant { EventId = events[0].Id, UserId = bob.Id, JoinDate = DateTime.UtcNow },
        new EventParticipant { EventId = events[1].Id, UserId = alice.Id, JoinDate = DateTime.UtcNow },
        new EventParticipant { EventId = events[2].Id, UserId = bob.Id, JoinDate = DateTime.UtcNow }
    };


        _context.EventParticipants.AddRange(eventParticipants);
        await _context.SaveChangesAsync();
    }
}