namespace GymTracker.Models;

public class Workout
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public required List<WorkoutType> WorkoutType { get; set; }
    public required BodyTarget BodyTarget { get; set; }

    public required Equipment Equipment { get; set; }
    public int Duration { get; set; }
    public Intensity Intensity { get; set; }

    public bool UserCreated { get; set; }
    public bool ExcludeByDefault { get; set; }
    public int TimesPerformed { get; set; }

    //Sessions that include this workout
    public List<SessionWorkout> SessionWorkouts { get; set; } = new();
}

public enum Intensity
{
    LOW,
    MID,
    HIGH
}

public enum WorkoutType
{
    CARDIO,
    STRENGTH,
    MUSCLE,
    POWER,
    ENDURANCE,
    CORE,
    PILATES,
    HIIT,
    FLEXIBILITY,
    WARMUP,
    COOLDOWN
}

public enum Equipment
{
    FREEWEIGHTS,
    BENCH,
    CABLES,
    MACHINE,
    FLOOR,
    TREADMILL
}

public enum BodyTarget
{
    CHEST,
    CORE,
    BACK,
    SHOULDERS,
    ARMS,
    LEGS
}