using GymTracker.Data;
using GymTracker.Models;
using Microsoft.AspNetCore.Components;
using GymTracker.Services;

namespace GymTracker.Components.Pages;

public partial class Workouts
{
    [Inject] private IWorkoutService Service { get; set; } = default!;

    private List<Workout> allWorkouts = new();

    private string nameFilter = "";
    private string typeFilter = "";
    private string bodyFilter = "";
    private string equipmentFilter = "";
    private string intensityFilter = "";
    private string sourceFilter = "";
    private string statusFilter = "included";
    private int? minDuration;
    private int? maxDuration;

    protected override void OnInitialized()
    {
        allWorkouts = Service.GetAll();
    }

    private IEnumerable<Workout> FilteredWorkouts =>
        allWorkouts.Where(w =>
            (string.IsNullOrWhiteSpace(nameFilter) ||
                w.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase)) &&
            (typeFilter == "" || w.WorkoutType.Any(t => t.ToString() == typeFilter)) &&
            (bodyFilter == "" || w.BodyTarget.ToString() == bodyFilter) &&
            (equipmentFilter == "" || w.Equipment.ToString() == equipmentFilter) &&
            (intensityFilter == "" || w.Intensity.ToString() == intensityFilter) &&
            (sourceFilter == "" || w.UserCreated == (sourceFilter == "user")) &&
            (statusFilter == "all" || w.ExcludeByDefault == (statusFilter == "excluded")) &&
            (minDuration == null || w.Duration >= minDuration) &&
            (maxDuration == null || w.Duration <= maxDuration));

    private static string IntensityClass(Intensity i) => i switch
    {
        Intensity.LOW => "low",
        Intensity.MID => "mid",
        _ => "high"
    };

    private void ToggleExclude(Workout w)
    {
        w.ExcludeByDefault = !w.ExcludeByDefault;
        Service.Update(w);
    }

    private void ClearFilters()
    {
        nameFilter = typeFilter = bodyFilter = equipmentFilter = intensityFilter = sourceFilter = "";
        statusFilter = "included";
        minDuration = maxDuration = null;
    }

    private void DeleteWorkout(Workout w)
    {
        Service.Delete(w);
        allWorkouts.Remove(w);
    }
}