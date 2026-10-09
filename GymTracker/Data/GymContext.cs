using Microsoft.EntityFrameworkCore;
using GymTracker.Models;

namespace GymTracker.Data;

public class GymContext : DbContext
{
    public GymContext() { }
    public GymContext(DbContextOptions<GymContext> options) : base(options) { }
    public DbSet<Workout> Workouts { get; set; }

    public DbSet<Session> Sessions { get; set; }
    public DbSet<SessionWorkout> SessionWorkouts { get; set; }

    //session tables and configured relationshios, duplicate prevention and restricted workout deletion
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Session>(entity =>
        {
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Notes).HasMaxLength(2000);
            entity.Property(s => s.Status).HasConversion<string>().HasMaxLength(20);
            entity.HasIndex(s => new { s.IsArchived, s.Date });
        });
        modelBuilder.Entity<SessionWorkout>(entity =>
        {
            // Prevent the same workout appearing twice in one session.
            entity.HasKey(sw => new { sw.SessionId, sw.WorkoutId });
            entity.HasOne(sw => sw.Session)
                .WithMany(s => s.SessionWorkouts)
                .HasForeignKey(sw => sw.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
            // Historical sessions must retain their referenced workout.
            entity.HasOne(sw => sw.Workout)
                .WithMany(w => w.SessionWorkouts)
                .HasForeignKey(sw => sw.WorkoutId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        // No global archive filters: archived history must remain readable.
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=gymtracker.db");
    }
}