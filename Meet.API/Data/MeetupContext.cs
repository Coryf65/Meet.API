﻿using Microsoft.EntityFrameworkCore;
using Meet.API.Entities;

namespace Meet.API.Data;

public class MeetupContext : DbContext
{
    private readonly DbContextOptions<MeetupContext> _context;

    public DbSet<Meetup> Meetups { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Lecture> Lectures { get; set; }

    public MeetupContext(DbContextOptions<MeetupContext> options) : base(options)
    {
        _context = options;
	}

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
}