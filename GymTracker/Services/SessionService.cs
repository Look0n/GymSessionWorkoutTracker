using GymTracker.Data;
using GymTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Services;

public class SessionService : ISessionService
{
    private readonly GymContext _db;
    private readonly TimeProvider _clock;

    public SessionService(GymContext db, TimeProvider clock)
    {
        _db = db;
        _clock = clock;
    }

    private IQueryable<Session> WithWorkouts() => _db.Sessions
        .Include(s => s.SessionWorkouts).ThenInclude(sw => sw.Workout);

    // Read-only copies prevent unsaved form edits changing tracked entities.
    public List<Session> GetAll() => WithWorkouts().AsNoTracking()
        .Where(s => !s.IsArchived && s.Status != SessionStatus.Completed)
        .OrderByDescending(s => s.Date).ToList();

    // Completed sessions plus archived plans/records, available for viewing.
    public List<Session> GetHistory() => WithWorkouts().AsNoTracking()
        .Where(s => s.Status == SessionStatus.Completed || s.IsArchived)
        .OrderByDescending(s => s.Date).ToList();

    public Session? GetById(int id) => WithWorkouts().AsNoTracking()
        .SingleOrDefault(s => s.Id == id);

    // Overload 1: plan a session without duration or fabricated timestamps.
    public Session CreateSession(string name, DateTime date, List<int> workoutIds)
        => Create(name, date, workoutIds, null);

    // Overload 2: record an already completed session using manual duration.
    public Session CreateSession(string name, DateTime date, List<int> workoutIds,
        double durationMinutes)
    {
        ValidateDuration(durationMinutes);
        return Create(name, date, workoutIds, durationMinutes);
    }

    private Session Create(string name, DateTime date, List<int> workoutIds,
        double? durationMinutes)
    {
        ValidateDetails(name, date, null);
        var ids = ValidateWorkouts(workoutIds);
        var session = new Session
        {
            Name = name.Trim(),
            Date = date,
            Status = durationMinutes.HasValue ? SessionStatus.Completed : SessionStatus.Planned,
            DurationMinutes = durationMinutes,
            SessionWorkouts = ids.Select((id, index) => new SessionWorkout
            {
                WorkoutId = id,
                Order = index + 1,
                // A historical record initially treats the selected workouts as done.
                // Users can correct individual completion afterwards.
                IsCompleted = durationMinutes.HasValue
            }).ToList()
        };
        _db.Sessions.Add(session);
        _db.SaveChanges();
        return GetById(session.Id)!;
    }

    public void Update(int id, string name, DateTime date, List<int> workoutIds,
        string? notes = null, double? durationMinutes = null)
    {
        var session = GetEditable(id);
        ValidateDetails(name, date, notes);
        var ids = ValidateWorkouts(workoutIds,
            session.SessionWorkouts.Select(sw => sw.WorkoutId));
        if (durationMinutes.HasValue)
        {
            if (session.Status != SessionStatus.Completed)
                throw new InvalidOperationException("Only completed sessions have an editable duration.");
            ValidateDuration(durationMinutes.Value);
        }

        // Validate everything before mutating tracked records.
        session.Name = name.Trim();
        session.Date = date;
        session.Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        if (durationMinutes.HasValue)
            session.DurationMinutes = durationMinutes.Value;

        var removed = session.SessionWorkouts.Where(sw => !ids.Contains(sw.WorkoutId)).ToList();
        foreach (var association in removed)
        {
            session.SessionWorkouts.Remove(association);
            _db.SessionWorkouts.Remove(association);
        }
        for (int index = 0; index < ids.Count; index++)
        {
            var association = session.SessionWorkouts.SingleOrDefault(sw => sw.WorkoutId == ids[index]);
            if (association == null)
            {
                association = new SessionWorkout { WorkoutId = ids[index] };
                session.SessionWorkouts.Add(association);
            }
            association.Order = index + 1;
        }
        // One SaveChanges atomically saves the session and all associations.
        // Existing completion flags and live timestamps remain intact.
        _db.SaveChanges();
    }

    public void Start(int id)
    {
        var session = GetEditable(id);
        if (session.Status != SessionStatus.Planned)
            throw new InvalidOperationException("Only planned sessions can be started.");
        RequireWorkouts(session);
        session.StartedAtUtc = _clock.GetUtcNow().UtcDateTime;
        session.Status = SessionStatus.InProgress;
        _db.SaveChanges();
    }

    public void Complete(int id)
    {
        var session = GetEditable(id);
        if (session.Status != SessionStatus.InProgress || !session.StartedAtUtc.HasValue)
            throw new InvalidOperationException("Start the session before completing it.");
        RequireWorkouts(session);
        var completedAt = _clock.GetUtcNow().UtcDateTime;
        var minutes = (completedAt - session.StartedAtUtc.Value).TotalMinutes;
        ValidateDuration(minutes);
        session.CompletedAtUtc = completedAt;
        session.DurationMinutes = minutes;
        session.Status = SessionStatus.Completed;
        // Completion flags remain unchanged: partially completed sessions are allowed.
        _db.SaveChanges();
    }

    public void SetWorkoutCompletion(int sessionId, int workoutId, bool isCompleted)
    {
        var session = GetEditable(sessionId);
        if (session.Status == SessionStatus.Planned)
            throw new InvalidOperationException("Start the session before marking workouts complete.");
        var association = session.SessionWorkouts.SingleOrDefault(sw => sw.WorkoutId == workoutId)
            ?? throw new InvalidOperationException("This workout is not included in the session.");
        association.IsCompleted = isCompleted;
        _db.SaveChanges();
    }

    public void Archive(int id)
    {
        var session = WithWorkouts().SingleOrDefault(s => s.Id == id)
            ?? throw new InvalidOperationException("Session not found.");
        // Repeated archive requests are harmless. Archiving does not imply completion.
        session.IsArchived = true;
        _db.SaveChanges();
    }

    private Session GetEditable(int id)
    {
        var session = WithWorkouts().SingleOrDefault(s => s.Id == id)
            ?? throw new InvalidOperationException("Session not found.");
        if (session.IsArchived)
            throw new InvalidOperationException("Archived sessions are read-only.");
        return session;
    }

    private List<int> ValidateWorkouts(List<int> workoutIds,
        IEnumerable<int>? existingWorkoutIds = null)
    {
        ArgumentNullException.ThrowIfNull(workoutIds);
        if (workoutIds.Count == 0)
            throw new ArgumentException("Select at least one workout.");
        if (workoutIds.Distinct().Count() != workoutIds.Count)
            throw new ArgumentException("A workout cannot appear twice in the same session.");
        if (_db.Workouts.Count(w => workoutIds.Contains(w.Id)) != workoutIds.Count)
            throw new ArgumentException("One or more selected workouts no longer exist.");
        var existingIds = existingWorkoutIds?.ToHashSet() ?? new HashSet<int>();
        var newIds = workoutIds.Where(id => !existingIds.Contains(id)).ToList();
        if (_db.Workouts.Any(w => newIds.Contains(w.Id) && w.ExcludeByDefault))
            throw new ArgumentException("Excluded workouts cannot be added to a session.");
        return workoutIds.ToList();
    }

    private static void ValidateDetails(string name, DateTime date, string? notes)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 100)
            throw new ArgumentException("Session name is required and must be at most 100 characters.");
        if (date == default)
            throw new ArgumentException("Session date is required.");
        if (notes?.Trim().Length > 2000)
            throw new ArgumentException("Notes must be at most 2000 characters.");
    }

    private static void ValidateDuration(double durationMinutes)
    {
        if (!double.IsFinite(durationMinutes) || durationMinutes <= 0)
            throw new ArgumentException("Duration must be a finite number greater than zero.");
    }

    private static void RequireWorkouts(Session session)
    {
        if (session.SessionWorkouts.Count == 0)
            throw new InvalidOperationException("The session must contain at least one workout.");
    }
}
