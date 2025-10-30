using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventParticipant> EventParticipants { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<EventParticipant>()
            .HasKey(ep => new { ep.EventId, ep.UserId }); 

        modelBuilder.Entity<EventParticipant>()
            .HasOne(ep => ep.Event)
            .WithMany(e => e.Participants)
            .HasForeignKey(ep => ep.EventId);

        modelBuilder.Entity<EventParticipant>()
            .HasOne(ep => ep.User)
            .WithMany(u => u.Participations)
            .HasForeignKey(ep => ep.UserId);

        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique(); 
        });

        
        var passwordHash1 = "$2a$10$wU0T5zJ1z2z3z4z5z6z7z8z9z0z1z2z3z4z5z"; 
        var passwordHash2 = "$2a$10$xU0T5zJ1z2z3z4z5z6z7z8z9z0z1z2z3z4z5z"; 

       
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Email = "alice.org@event.com", PasswordHash = passwordHash1, Username = "Alice" },
            new User { Id = 2, Email = "bob.user@event.com", PasswordHash = passwordHash2, Username = "Bob" }
        );

        
        modelBuilder.Entity<Event>().HasData(
            new Event
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
                Capacity = null, // Unlimited
                IsPublic = true
            },
            new Event
            {
                Id = 4, 
                HostId = 1,
                Title = "Private Team Dinner",
                Description = "Team building event (Private).",
                DateTime = DateTime.UtcNow.AddDays(10),
                Location = "Italian Restaurant",
                Capacity = 10,
                IsPublic = false
            }
        );

       
        modelBuilder.Entity<EventParticipant>().HasData(
           
            new EventParticipant { EventId = 1, UserId = 1 }, // Alice
            new EventParticipant { EventId = 1, UserId = 2 }, // Bob
         
            new EventParticipant { EventId = 2, UserId = 1 },

            new EventParticipant { EventId = 3, UserId = 2 }
        );
    }
}

