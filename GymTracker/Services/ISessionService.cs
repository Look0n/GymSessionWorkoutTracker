using GymTracker.Models;

namespace GymTracker.Services;

public interface ISessionService
{
    List<Session> GetAll();
    List<Session> GetHistory();
    Session? GetById(int id);

    Session CreateSession(string name, DateTime date, List<int> workoutIds);
    Session CreateSession(string name, DateTime date, List<int> workoutIds,
        double durationMinutes);

    void Update(int id, string name, DateTime date, List<int> workoutIds,
        string? notes = null, double? durationMinutes = null);
    void Start(int id);
    void Complete(int id);
    void SetWorkoutCompletion(int sessionId, int workoutId, bool isCompleted);
    void Archive(int id);
}
