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
}