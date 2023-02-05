using Microsoft.EntityFrameworkCore;
using Meet.API.Entities;

namespace Meet.API.Data;

public class MeetupContext : DbContext
{
    public DbSet<Meetup> Meetups { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    public MeetupContext(DbContextOptions<MeetupContext> options) : base(options)
    {

	}

    /// <summary>
    /// Create Relationships in Tables of DB
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meetup>()
            .HasOne(c => c.CreatedBy);

		modelBuilder.Entity<User>()
			.HasOne(u => u.Role);

		// setting up relationships, one location one meetup
		modelBuilder.Entity<Meetup>()
            .HasOne(m => m.Location)
            .WithOne(l => l.Meetup)
            .HasForeignKey<Location>(l => l.MeetupId);

        // setting up relationships, many lectures in one meetup
        modelBuilder.Entity<Meetup>()
            .HasMany(m => m.Lectures)
            .WithOne(l => l.Meetup);        
    }
}