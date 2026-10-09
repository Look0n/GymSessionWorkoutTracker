using GymTracker.Models;

namespace GymTracker.Data;

public static class SeedData
{
    public static void Initialize(GymContext db)
    {
        // Only seed an empty table
        if (db.Workouts.Any()) return;

        var workouts = new List<Workout>
        {
            // Cardio
            new()
            {
                Name = "Treadmill Run",
                Description = "Steady-paced run on the treadmill to build aerobic fitness and stamina.",
                WorkoutType = [WorkoutType.CARDIO, WorkoutType.ENDURANCE],
                BodyTarget = BodyTarget.LEGS, Equipment = Equipment.TREADMILL, Duration = 30, Intensity = Intensity.MID
            },
            new()
            {
                Name = "Treadmill Sprints",
                Description = "Short, all-out sprints separated by walking recovery to spike your heart rate.",
                WorkoutType = [WorkoutType.HIIT, WorkoutType.CARDIO],
                BodyTarget = BodyTarget.LEGS, Equipment = Equipment.TREADMILL, Duration = 20, Intensity = Intensity.HIGH
            },

            // Chest
            new()
            {
                Name = "Bench Press",
                Description = "Lie on a bench and press a loaded barbell up from your chest. A staple for chest strength.",
                WorkoutType = [WorkoutType.STRENGTH, WorkoutType.MUSCLE],
                BodyTarget = BodyTarget.CHEST, Equipment = Equipment.BENCH, Duration = 25, Intensity = Intensity.HIGH
            },
            new()
            {
                Name = "Incline Dumbbell Press",
                Description = "Press dumbbells upward on an inclined bench to target the upper chest.",
                WorkoutType = [WorkoutType.MUSCLE],
                BodyTarget = BodyTarget.CHEST, Equipment = Equipment.BENCH, Duration = 20, Intensity = Intensity.MID
            },
            new()
            {
                Name = "Push-Ups",
                Description = "Bodyweight press from the floor working the chest, shoulders and triceps.",
                WorkoutType = [WorkoutType.STRENGTH, WorkoutType.ENDURANCE],
                BodyTarget = BodyTarget.CHEST, Equipment = Equipment.FLOOR, Duration = 10, Intensity = Intensity.MID
            },

            // Core
            new()
            {
                Name = "Plank",
                Description = "Hold a straight-body position on your forearms and toes to build core endurance.",
                WorkoutType = [WorkoutType.CORE, WorkoutType.ENDURANCE],
                BodyTarget = BodyTarget.CORE, Equipment = Equipment.FLOOR, Duration = 5, Intensity = Intensity.MID
            },
            new()
            {
                Name = "Cable Woodchop",
                Description = "Rotate your torso while pulling a cable diagonally across the body to train rotational core power.",
                WorkoutType = [WorkoutType.CORE, WorkoutType.POWER],
                BodyTarget = BodyTarget.CORE, Equipment = Equipment.CABLES, Duration = 10, Intensity = Intensity.MID
            },
            new()
            {
                Name = "Pilates Hundred",
                Description = "Lie on your back with legs raised and pulse your arms while breathing in a steady rhythm.",
                WorkoutType = [WorkoutType.PILATES, WorkoutType.CORE],
                BodyTarget = BodyTarget.CORE, Equipment = Equipment.FLOOR, Duration = 10, Intensity = Intensity.LOW
            },

            // Back
            new()
            {
                Name = "Lat Pulldown",
                Description = "Pull a bar down to your upper chest on a machine to work the lats and upper back.",
                WorkoutType = [WorkoutType.MUSCLE],
                BodyTarget = BodyTarget.BACK, Equipment = Equipment.MACHINE, Duration = 15, Intensity = Intensity.MID
            },
            new()
            {
                Name = "Seated Cable Row",
                Description = "Sit upright and pull a cable handle toward your torso to strengthen the mid back.",
                WorkoutType = [WorkoutType.STRENGTH, WorkoutType.MUSCLE],
                BodyTarget = BodyTarget.BACK, Equipment = Equipment.CABLES, Duration = 15, Intensity = Intensity.MID
            },
            new()
            {
                Name = "Deadlift",
                Description = "Lift a loaded barbell from the floor to hip height. Works the whole back, glutes and hamstrings.",
                WorkoutType = [WorkoutType.STRENGTH, WorkoutType.POWER],
                BodyTarget = BodyTarget.BACK, Equipment = Equipment.FREEWEIGHTS, Duration = 25, Intensity = Intensity.HIGH
            },

            // Shoulders
            new()
            {
                Name = "Overhead Press",
                Description = "Press a barbell or dumbbells from shoulder height to overhead to build shoulder strength.",
                WorkoutType = [WorkoutType.STRENGTH],
                BodyTarget = BodyTarget.SHOULDERS, Equipment = Equipment.FREEWEIGHTS, Duration = 20, Intensity = Intensity.HIGH
            },
            new()
            {
                Name = "Lateral Raises",
                Description = "Raise light dumbbells out to the sides to the height of your shoulders to build shoulder width.",
                WorkoutType = [WorkoutType.MUSCLE],
                BodyTarget = BodyTarget.SHOULDERS, Equipment = Equipment.FREEWEIGHTS, Duration = 10, Intensity = Intensity.LOW
            },
            new()
            {
                Name = "Face Pulls",
                Description = "Pull a rope attachment toward your face to activate the rear shoulders. Good for posture and warming up.",
                WorkoutType = [WorkoutType.MUSCLE, WorkoutType.WARMUP],
                BodyTarget = BodyTarget.SHOULDERS, Equipment = Equipment.CABLES, Duration = 10, Intensity = Intensity.LOW
            },

            // Arms
            new()
            {
                Name = "Bicep Curls",
                Description = "Curl dumbbells from your hips up to your shoulders to build the front of the upper arm.",
                WorkoutType = [WorkoutType.MUSCLE],
                BodyTarget = BodyTarget.ARMS, Equipment = Equipment.FREEWEIGHTS, Duration = 15, Intensity = Intensity.MID
            },
            new()
            {
                Name = "Tricep Pushdowns",
                Description = "Push a cable attachment down until your arms are straight to isolate the triceps.",
                WorkoutType = [WorkoutType.MUSCLE],
                BodyTarget = BodyTarget.ARMS, Equipment = Equipment.CABLES, Duration = 10, Intensity = Intensity.MID
            },

            // Legs
            new()
            {
                Name = "Back Squat",
                Description = "Squat down with a barbell across your upper back, then drive up. Builds legs, glutes and core.",
                WorkoutType = [WorkoutType.STRENGTH, WorkoutType.POWER],
                BodyTarget = BodyTarget.LEGS, Equipment = Equipment.FREEWEIGHTS, Duration = 30, Intensity = Intensity.HIGH
            },
            new()
            {
                Name = "Leg Press",
                Description = "Push a weighted platform away with your feet while seated, a controlled way to load the legs.",
                WorkoutType = [WorkoutType.STRENGTH, WorkoutType.MUSCLE],
                BodyTarget = BodyTarget.LEGS, Equipment = Equipment.MACHINE, Duration = 20, Intensity = Intensity.HIGH
            },
            new()
            {
                Name = "Box Jumps",
                Description = "Explosively jump onto a sturdy box and step back down to build leg power.",
                WorkoutType = [WorkoutType.POWER, WorkoutType.HIIT],
                BodyTarget = BodyTarget.LEGS, Equipment = Equipment.FLOOR, Duration = 10, Intensity = Intensity.HIGH
            },
            new()
            {
                Name = "Hamstring Stretch",
                Description = "Extend one leg and reach toward your toes to lengthen the back of the thigh after training.",
                WorkoutType = [WorkoutType.FLEXIBILITY, WorkoutType.COOLDOWN],
                BodyTarget = BodyTarget.LEGS, Equipment = Equipment.FLOOR, Duration = 10, Intensity = Intensity.LOW
            },

            // Warm-up / cooldown
            new()
            {
                Name = "Arm Circles",
                Description = "Make slow circles with straight arms to loosen the shoulders before lifting.",
                WorkoutType = [WorkoutType.WARMUP, WorkoutType.FLEXIBILITY],
                BodyTarget = BodyTarget.SHOULDERS, Equipment = Equipment.FLOOR, Duration = 5, Intensity = Intensity.LOW
            },
            new()
            {
                Name = "Child's Pose",
                Description = "Kneel and fold forward with arms outstretched to gently stretch the back and relax after a session.",
                WorkoutType = [WorkoutType.COOLDOWN, WorkoutType.FLEXIBILITY],
                BodyTarget = BodyTarget.BACK, Equipment = Equipment.FLOOR, Duration = 5, Intensity = Intensity.LOW
            },
        };

        db.Workouts.AddRange(workouts);
        db.SaveChanges();
    }

    public static void InitializeSessions(GymContext db)
{
    // Only seed an empty table
    if (db.Sessions.Any()) return;

    var workouts = db.Workouts.ToDictionary(w => w.Name, w => w.Id);

    // Link each session to existing workouts by name.
    SessionWorkout Link(string name, int order, bool completed = false)
    {
        return new SessionWorkout
        {
            WorkoutId = workouts[name],
            Order = order,
            IsCompleted = completed
        };
    }

    var sessions = new List<Session>
    {
        // Current sessions
        new()
        {
            Name = "Chest and Triceps",
            Date = DateTime.Today.AddDays(1),
            Status = SessionStatus.Planned,
            Notes = "Upper-body strength session.",
            SessionWorkouts =
            [
                Link("Arm Circles", 1),
                Link("Bench Press", 2),
                Link("Incline Dumbbell Press", 3),
                Link("Tricep Pushdowns", 4)
            ]
        },
        new()
        {
            Name = "Lower Body Strength",
            Date = DateTime.Today.AddDays(3),
            Status = SessionStatus.Planned,
            Notes = "Leg strength training with a cooldown.",
            SessionWorkouts =
            [
                Link("Back Squat", 1),
                Link("Leg Press", 2),
                Link("Hamstring Stretch", 3)
            ]
        },
        new()
        {
            Name = "Back and Core",
            Date = DateTime.Today.AddDays(5),
            Status = SessionStatus.Planned,
            Notes = "Back exercises followed by core training.",
            SessionWorkouts =
            [
                Link("Lat Pulldown", 1),
                Link("Seated Cable Row", 2),
                Link("Plank", 3),
                Link("Child's Pose", 4)
            ]
        },

        // History: chest
        new()
        {
            Name = "Chest Strength",
            Date = DateTime.Today.AddDays(-1),
            Status = SessionStatus.Completed,
            DurationMinutes = 70,
            Notes = "Completed all exercises, including rest between sets.",
            SessionWorkouts =
            [
                Link("Arm Circles", 1, true),
                Link("Bench Press", 2, true),
                Link("Incline Dumbbell Press", 3, true),
                Link("Push-Ups", 4, true)
            ]
        },

        // History: legs
        new()
        {
            Name = "Leg Strength",
            Date = DateTime.Today.AddDays(-3),
            Status = SessionStatus.Completed,
            DurationMinutes = 75,
            Notes = "Completed squat and leg press training with a cooldown.",
            SessionWorkouts =
            [
                Link("Back Squat", 1, true),
                Link("Leg Press", 2, true),
                Link("Hamstring Stretch", 3, true)
            ]
        },

        // History: core
        new()
        {
            Name = "Core Conditioning",
            Date = DateTime.Today.AddDays(-5),
            Status = SessionStatus.Completed,
            DurationMinutes = 35,
            Notes = "Focused on controlled movement and core endurance.",
            SessionWorkouts =
            [
                Link("Plank", 1, true),
                Link("Cable Woodchop", 2, true),
                Link("Pilates Hundred", 3, true)
            ]
        },

        // History: back — partially completed
        new()
        {
            Name = "Back and Pulling",
            Date = DateTime.Today.AddDays(-7),
            Status = SessionStatus.Completed,
            DurationMinutes = 40,
            Notes = "Finished early; deadlifts were not completed.",
            SessionWorkouts =
            [
                Link("Lat Pulldown", 1, true),
                Link("Seated Cable Row", 2, true),
                Link("Deadlift", 3)
            ]
        },

        // History: shoulders
        new()
        {
            Name = "Shoulder Training",
            Date = DateTime.Today.AddDays(-9),
            Status = SessionStatus.Completed,
            DurationMinutes = 55,
            Notes = "Completed shoulder exercises after warming up.",
            SessionWorkouts =
            [
                Link("Arm Circles", 1, true),
                Link("Overhead Press", 2, true),
                Link("Lateral Raises", 3, true),
                Link("Face Pulls", 4, true)
            ]
        },

        // History: cardio
        new()
        {
            Name = "Cardio and Flexibility",
            Date = DateTime.Today.AddDays(-12),
            Status = SessionStatus.Completed,
            DurationMinutes = 50,
            Notes = "Steady treadmill run followed by stretching.",
            SessionWorkouts =
            [
                Link("Treadmill Run", 1, true),
                Link("Hamstring Stretch", 2, true),
                Link("Child's Pose", 3, true)
            ]
        },

        // History: arms — partially completed
        new()
        {
            Name = "Arms Express",
            Date = DateTime.Today.AddDays(-15),
            Status = SessionStatus.Completed,
            DurationMinutes = 35,
            Notes = "Completed arm exercises but skipped the final push-ups.",
            SessionWorkouts =
            [
                Link("Bicep Curls", 1, true),
                Link("Tricep Pushdowns", 2, true),
                Link("Push-Ups", 3)
            ]
        },

        // History: power
        new()
        {
            Name = "Lower Body Power",
            Date = DateTime.Today.AddDays(-18),
            Status = SessionStatus.Completed,
            DurationMinutes = 65,
            Notes = "Power training with rest between exercises.",
            SessionWorkouts =
            [
                Link("Treadmill Sprints", 1, true),
                Link("Box Jumps", 2, true),
                Link("Leg Press", 3, true),
                Link("Hamstring Stretch", 4, true)
            ]
        },

        // Archived history: recovery
        new()
        {
            Name = "Core and Recovery",
            Date = DateTime.Today.AddDays(-21),
            Status = SessionStatus.Completed,
            DurationMinutes = 25,
            IsArchived = true,
            Notes = "Archived recovery session.",
            SessionWorkouts =
            [
                Link("Pilates Hundred", 1, true),
                Link("Plank", 2, true),
                Link("Child's Pose", 3, true)
            ]
        },

        // Archived history: back — partially completed
        new()
        {
            Name = "Back Strength",
            Date = DateTime.Today.AddDays(-28),
            Status = SessionStatus.Completed,
            DurationMinutes = 55,
            IsArchived = true,
            Notes = "Completed lifting exercises but skipped the cooldown.",
            SessionWorkouts =
            [
                Link("Deadlift", 1, true),
                Link("Seated Cable Row", 2, true),
                Link("Lat Pulldown", 3, true),
                Link("Child's Pose", 4)
            ]
        }
    };

    db.Sessions.AddRange(sessions);
    db.SaveChanges();
}
}