namespace GymTracker.Models;

// One existing workout selected for one session.
public class SessionWorkout
{
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;
    public int WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;
    public int Order { get; set; }
    public bool IsCompleted { get; set; }
}
