namespace GymTracker.Models;

public class Session
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }

    // Scheduled date for a plan, or the actual date for a historical record.
    public DateTime Date { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.Planned;

    // Store live tracking timestamps in UTC. Elapsed time includes rest.
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }

    // Null until completed; live elapsed minutes or a manually entered duration.
    // Decimal minutes avoid rounding a short session down to zero.
    public double? DurationMinutes { get; set; }
    public bool IsArchived { get; set; }

    public List<SessionWorkout> SessionWorkouts { get; set; } = new();
}

public enum SessionStatus
{
    Planned = 0,
    InProgress = 1,
    Completed = 2
}
