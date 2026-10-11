using GymTracker.Data;
using GymTracker.Models;
using GymTracker.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Tests;

[TestFixture]
public class SessionServiceTests
{
    private SqliteConnection _connection = default!;
    private GymContext _db = default!;
    private SessionService _service = default!;
    private TestClock _clock = default!;
    private List<int> _ids = default!;

    private sealed class TestClock : TimeProvider
    {
        public DateTimeOffset Now { get; set; } = new(2026, 10, 9, 0, 0, 0, TimeSpan.Zero);
        public override DateTimeOffset GetUtcNow() => Now;
    }

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _db = new GymContext(new DbContextOptionsBuilder<GymContext>()
            .UseSqlite(_connection).Options);
        _db.Database.EnsureCreated();
        SeedData.InitaliseWorkouts(_db); //required by the sessions
        _ids = _db.Workouts.OrderBy(w => w.Id).Take(2).Select(w => w.Id).ToList();
        _clock = new TestClock();
        _service = new SessionService(_db, _clock);
    }

    [TearDown]
    public void TearDown()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    [Test]
    public void Create_Planned()
    {
        var session = _service.CreateSession("Plan", DateTime.Today, _ids);
        Assert.That(session.Status, Is.EqualTo(SessionStatus.Planned));
        Assert.That(session.DurationMinutes, Is.Null);
        Assert.That(session.SessionWorkouts.Select(sw => sw.Order), Is.EquivalentTo(new[] { 1, 2 }));
    }

    [Test]
    public void Create_Historical()
    {
        var session = _service.CreateSession("Record", DateTime.Today, _ids, 50);
        Assert.That(session.Status, Is.EqualTo(SessionStatus.Completed));
        Assert.That(session.DurationMinutes, Is.EqualTo(50));
        Assert.That(session.StartedAtUtc, Is.Null);
        Assert.That(session.SessionWorkouts.All(sw => sw.IsCompleted), Is.True);
    }

    [Test]
    public void Complete_PartialSession()
    {
        var session = _service.CreateSession("Live", DateTime.Today, _ids);
        _service.Start(session.Id);
        _service.SetWorkoutCompletion(session.Id, _ids[0], true);
        _clock.Now = _clock.Now.AddMinutes(50);
        _service.Complete(session.Id);
        var saved = _service.GetById(session.Id)!;
        Assert.That(saved.DurationMinutes, Is.EqualTo(50));
        Assert.That(saved.Status, Is.EqualTo(SessionStatus.Completed));
        Assert.That(saved.SessionWorkouts.Count(sw => sw.IsCompleted), Is.EqualTo(1));
        Assert.Throws<InvalidOperationException>(() => _service.Start(session.Id));
    }

    [Test]
    public void Complete_Unstarted_Throws()
    {
        var session = _service.CreateSession("Plan", DateTime.Today, _ids);
        Assert.Throws<InvalidOperationException>(() => _service.Complete(session.Id));
    }

    [Test]
    public void Update_CompletedSession()
    {
        var session = _service.CreateSession("Record", DateTime.Today, _ids, 50);
        var thirdId = _db.Workouts.OrderBy(w => w.Id).Skip(2).First().Id;
        _service.Update(session.Id, "Corrected", DateTime.Today,
            new List<int> { _ids[0], thirdId }, "Notes", 55);
        var saved = _service.GetById(session.Id)!;
        Assert.That(saved.Name, Is.EqualTo("Corrected"));
        Assert.That(saved.DurationMinutes, Is.EqualTo(55));
        Assert.That(saved.SessionWorkouts.Single(sw => sw.WorkoutId == _ids[0]).IsCompleted, Is.True);
        Assert.That(saved.SessionWorkouts.Single(sw => sw.WorkoutId == thirdId).IsCompleted, Is.False);
        Assert.That(saved.SessionWorkouts.Any(sw => sw.WorkoutId == _ids[1]), Is.False);
    }

    [Test]
    public void Archive_ReadOnlyHistory()
    {
        var session = _service.CreateSession("Plan", DateTime.Today, _ids);
        _service.Archive(session.Id);
        Assert.That(_service.GetAll(), Is.Empty);
        Assert.That(_service.GetHistory().Single().Id, Is.EqualTo(session.Id));
        Assert.Throws<InvalidOperationException>(() => _service.Start(session.Id));
        Assert.Throws<InvalidOperationException>(() =>
            _service.Update(session.Id, "Changed", DateTime.Today, _ids));
        Assert.That(_db.SessionWorkouts.Count(), Is.EqualTo(2));
    }

    [Test]
    public void Lists_SeparateCurrentAndHistory()
    {
        var planned = _service.CreateSession("Plan", DateTime.Today, _ids);
        var completed = _service.CreateSession("Record", DateTime.Today, _ids, 50);
        var archived = _service.CreateSession("Archived plan", DateTime.Today, _ids);
        _service.Archive(archived.Id);

        Assert.That(_service.GetAll().Select(s => s.Id), Is.EquivalentTo(new[] { planned.Id }));
        Assert.That(_service.GetHistory().Select(s => s.Id),
            Is.EquivalentTo(new[] { completed.Id, archived.Id }));
    }

    [Test]
    public void Create_ExcludedWorkout_Throws()
    {
        var workout = _db.Workouts.Find(_ids[0])!;
        workout.ExcludeByDefault = true;
        _db.SaveChanges();
        Assert.Throws<ArgumentException>(() => _service.CreateSession("Plan", DateTime.Today, _ids));
        Assert.That(_db.Sessions.Count(), Is.Zero);
    }

    [Test]
    public void Update_ExistingExcludedWorkoutAllowed()
    {
        var session = _service.CreateSession("Original", DateTime.Today, _ids);
        _db.Workouts.Find(_ids[0])!.ExcludeByDefault = true;
        _db.SaveChanges();
        _service.Update(session.Id, "Corrected", DateTime.Today, _ids);
        Assert.That(_service.GetById(session.Id)!.Name, Is.EqualTo("Corrected"));
        Assert.That(_service.GetById(session.Id)!.SessionWorkouts.Count, Is.EqualTo(2));
    }

    [Test]
    public void Update_NewExcludedWorkout_Throws()
    {
        var session = _service.CreateSession("Original", DateTime.Today, new List<int> { _ids[0] });
        _db.Workouts.Find(_ids[1])!.ExcludeByDefault = true;
        _db.SaveChanges();
        Assert.Throws<ArgumentException>(() => _service.Update(session.Id, "Changed", DateTime.Today, _ids));
        Assert.That(_service.GetById(session.Id)!.Name, Is.EqualTo("Original"));
        Assert.That(_service.GetById(session.Id)!.SessionWorkouts.Count, Is.EqualTo(1));
    }

    [Test]
    public void InvalidInput_DoesNotSave()
    {
        Assert.Throws<ArgumentException>(() => _service.CreateSession("Plan", DateTime.Today,
            new List<int> { _ids[0], _ids[0] }));
        Assert.Throws<ArgumentException>(() => _service.CreateSession("Record", DateTime.Today, _ids, 0));
        Assert.Throws<ArgumentException>(() => _service.CreateSession("Plan", DateTime.Today,
            new List<int> { int.MaxValue }));
        Assert.That(_db.Sessions.Count(), Is.Zero);
    }

    [Test]
    public void InvalidUpdate_PreservesDetails()
    {
        var session = _service.CreateSession("Original", DateTime.Today, _ids);
        Assert.Throws<ArgumentException>(() => _service.Update(session.Id, "Changed",
            DateTime.Today, new List<int> { int.MaxValue }));
        Assert.That(_service.GetById(session.Id)!.Name, Is.EqualTo("Original"));
        Assert.That(_db.SessionWorkouts.Count(), Is.EqualTo(2));
    }
}
