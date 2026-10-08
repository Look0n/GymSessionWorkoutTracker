using GymTracker.Models;

namespace GymTracker.Services;

public interface IWorkoutService
{
    List<Workout> GetAll();
    void Add(Workout workout);
    void Update(Workout workout);
    void Delete(Workout workout);
}