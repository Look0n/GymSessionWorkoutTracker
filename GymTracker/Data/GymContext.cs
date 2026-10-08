using Microsoft.EntityFrameworkCore;
using GymTracker.Models;

namespace GymTracker.Data;

public class GymContext : DbContext
{
    public GymContext() { }
    public GymContext(DbContextOptions<GymContext> options) : base(options) { }
    public DbSet<Workout> Workouts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=gymtracker.db");
    }
}