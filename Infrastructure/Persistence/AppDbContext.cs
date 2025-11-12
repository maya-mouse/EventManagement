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

    public DbSet<Tag> Tags { get; set; }
    public DbSet<EventParticipant> EventParticipants { get; set; }
    public DbSet<EventTag> EventTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>()
           .HasOne(e => e.Host)
           .WithMany(u => u.HostedEvents)
           .HasForeignKey(e => e.HostId)
           .OnDelete(DeleteBehavior.Cascade);

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
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<EventTag>()
            .HasKey(et => new { et.EventId, et.TagId });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasIndex(t => t.Name).IsUnique();
        });

        modelBuilder.Entity<EventTag>()
           .HasOne(et => et.Event)
           .WithMany(e => e.EventTags)
           .HasForeignKey(et => et.EventId);

        modelBuilder.Entity<EventTag>()
           .HasOne(et => et.Tag)
           .WithMany(t => t.EventTags)
           .HasForeignKey(et => et.TagId);

    }
}

