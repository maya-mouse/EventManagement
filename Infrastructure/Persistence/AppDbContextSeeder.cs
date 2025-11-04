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
            return;
        }

        var alicePassword = "pass123";
        var bobPassword = "123pass";

        var aliceHash = _passwordHasher.HashPassword(alicePassword);
        var bobHash = _passwordHasher.HashPassword(bobPassword);

        var users = new List<User>
        {
            new User { Email = "alice.org@event.com", PasswordHash = aliceHash, Username = "Alice" },
            new User { Email = "bob.user@event.com", PasswordHash = bobHash, Username = "Bob" }
        };

        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        var events = new List<Event>
        {    new Event
            {
                Id = 1,
                HostId = 1,
                Title = "Annual Tech Conference",
                Description = "Largest tech gathering of the year.",
                DateTime = DateTime.UtcNow.AddDays(30),
                Location = "Kyiv Expo Center",
                Capacity = 100,
                IsPublic = true
            },
            new Event
            {
                Id = 2,
                HostId = 1,
                Title = "Open Source Workshop",
                Description = "Hands-on coding session for contributors.",
                DateTime = DateTime.UtcNow.AddDays(45),
                Location = "Online via Zoom",
                Capacity = 50,
                IsPublic = true
            },
            new Event
            {
                Id = 3,
                HostId = 2,
                Title = "Local Book Club Meeting",
                Description = "Discussion on classic literature.",
                DateTime = DateTime.UtcNow.AddDays(15),
                Location = "Central Library",
                Capacity = null,
                IsPublic = true
            },
            new Event
            {
                Id = 4,
                HostId = 1,
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

        var eventParticipants = new List<EventParticipant>
        {
            new EventParticipant { EventId = 1, UserId = 1 },
            new EventParticipant { EventId = 1, UserId = 2 },
            new EventParticipant { EventId = 2, UserId = 1 },
            new EventParticipant { EventId = 3, UserId = 2 }
        };

        _context.EventParticipants.AddRange(eventParticipants);
        await _context.SaveChangesAsync();
    }
}