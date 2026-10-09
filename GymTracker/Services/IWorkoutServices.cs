using GymTracker.Models;

namespace GymTracker.Services;

public interface IWorkoutService
{
    List<Workout> GetAll();
    void Add(Workout workout);
    void Update(Workout workout);
    // Permanently deletes only workouts that are not referenced by any session.
    void Delete(Workout workout);
}