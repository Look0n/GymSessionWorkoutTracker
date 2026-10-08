using GymTracker.Data;
using GymTracker.Models;
using GymTracker.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Tests;

[TestFixture]
public class WorkoutServiceTests
{
    private SqliteConnection _connection = default!;
    private GymContext _db = default!;
    private IWorkoutService _service = default!;

    [SetUp]
    public void SetUp()
    {
        // In-memory SQLite lives only while the connection is open
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<GymContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new GymContext(options);
        _db.Database.EnsureCreated();
        _service = new WorkoutService(_db);
    }

    [TearDown]
    public void TearDown()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    private static Workout MakeWorkout(string name = "Test Workout") => new()
    {
        Name = name,
        WorkoutType = [WorkoutType.CARDIO, WorkoutType.HIIT],
        BodyTarget = BodyTarget.LEGS,
        Equipment = Equipment.FLOOR
    };

    [Test]
    public void Add_SavesWorkoutToDatabase()
    {
        _service.Add(MakeWorkout("Burpees"));

        var all = _service.GetAll();

        Assert.That(all, Has.Count.EqualTo(1));
        Assert.That(all[0].Name, Is.EqualTo("Burpees"));
    }

    [Test]
    public void Add_StoresWorkoutTypeList()
    {
        _service.Add(MakeWorkout());
        _db.ChangeTracker.Clear(); // force a fresh read from the database

        var saved = _service.GetAll().Single();

        Assert.That(saved.WorkoutType, Is.EquivalentTo(new[] { WorkoutType.CARDIO, WorkoutType.HIIT }));
    }

    [Test]
    public void NewWorkout_DefaultsAreFalseAndZero()
    {
        var workout = MakeWorkout();

        Assert.That(workout.UserCreated, Is.False);
        Assert.That(workout.ExcludeByDefault, Is.False);
        Assert.That(workout.TimesPerformed, Is.EqualTo(0));
    }

    [Test]
    public void Update_PersistsExcludeFlag()
    {
        var workout = MakeWorkout();
        _service.Add(workout);

        workout.ExcludeByDefault = true;
        _service.Update(workout);
        _db.ChangeTracker.Clear();

        Assert.That(_service.GetAll().Single().ExcludeByDefault, Is.True);
    }

    [Test]
    public void Delete_RemovesWorkout()
    {
        var workout = MakeWorkout();
        _service.Add(workout);

        _service.Delete(workout);

        Assert.That(_service.GetAll(), Is.Empty);
    }

    [Test]
    public void SeedData_AddsWorkoutsOnlyOnce()
    {
        SeedData.Initialize(_db);
        int firstCount = _service.GetAll().Count;

        SeedData.Initialize(_db); // second call should do nothing

        Assert.That(firstCount, Is.EqualTo(22));
        Assert.That(_service.GetAll(), Has.Count.EqualTo(firstCount));
    }
}