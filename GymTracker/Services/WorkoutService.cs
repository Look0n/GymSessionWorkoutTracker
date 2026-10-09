using GymTracker.Data;
using GymTracker.Models;

namespace GymTracker.Services;

public class WorkoutService : IWorkoutService
{
    private readonly GymContext _db;

    public WorkoutService(GymContext db)
    {
        _db = db;
    }

    public List<Workout> GetAll() => _db.Workouts.ToList();

    public void Add(Workout workout)
    {
        _db.Workouts.Add(workout);
        _db.SaveChanges();
    }

    public void Update(Workout workout)
    {
        _db.Workouts.Update(workout);
        _db.SaveChanges();
    }

    public void Delete(Workout workout)
    {
        // Include every session, including archived sessions, to protect history.
        if (_db.SessionWorkouts.Any(sw => sw.WorkoutId == workout.Id))
        {
            throw new InvalidOperationException(
                "This workout belongs to a session and cannot be deleted. Use Exclude instead.");
        }
        _db.Workouts.Remove(workout);
        _db.SaveChanges();
    }
}