using Microsoft.EntityFrameworkCore;

namespace Meet.API.Entities;

public class MeetupContext : DbContext
{
	//private readonly string _connectionString = "Server(localdb)\\mssqllocaldb;Database=MeetupDb;Trusted_Connection=True";
	private readonly string _connectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=Meetup;Integrated Security=SSPI;";

	public DbSet<Meetup> Meetups { get; set; }
	public DbSet<Location> Locations { get; set; }
	public DbSet<Lecture> Lectures { get; set; }

	/// <summary>
	/// Create Relationships in Tables of DB
	/// </summary>
	/// <param name="modelBuilder"></param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// setting up relationships, one location one meetup
		modelBuilder.Entity<Meetup>()
			.HasOne(m => m.Location)
			.WithOne(l => l.Meetup)
			.HasForeignKey<Location>(l => l.MeetupId);

		// setting up relationships, many lectures in one meetup
		modelBuilder.Entity<Meetup>()
			.HasMany(m => m.Lecture)
			.WithOne(l => l.Meetup);
	}

	/// <summary>
	/// Choose which Database to use
	/// </summary>
	/// <param name="optionsBuilder"></param>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(_connectionString);
	}
}