using System.ComponentModel.DataAnnotations;
using GymTracker.Models;
using Microsoft.AspNetCore.Components.Forms;
using GymTracker.Data;
using Microsoft.AspNetCore.Components;
using GymTracker.Services;

namespace GymTracker.Components.Pages;

public partial class CreateWorkout
{
    [Inject] private IWorkoutService Service { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    public class WorkoutForm
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(60, ErrorMessage = "Name must be 60 characters or fewer.")]
        public string Name { get; set; } = "";

        [StringLength(300, ErrorMessage = "Description must be 300 characters or fewer.")]
        public string? Description { get; set; }

        [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes.")]
        public int? Duration { get; set; }

        public BodyTarget BodyTarget { get; set; }
        public Equipment Equipment { get; set; }
        public Intensity Intensity { get; set; }
    }

    private WorkoutForm form = new();
    private EditContext editContext = default!;
    private HashSet<WorkoutType> selectedTypes = new();
    private string? typeError;

    protected override void OnInitialized()
    {
        editContext = new EditContext(form);
    }

    private void ToggleType(WorkoutType type, bool isChecked)
    {
        if (isChecked)
            selectedTypes.Add(type);
        else
            selectedTypes.Remove(type);

        if (selectedTypes.Any())
            typeError = null;
    }

    private void HandleSubmit()
    {
        bool formValid = editContext.Validate();
        typeError = selectedTypes.Any() ? null : "Pick at least one workout type.";

        if (!formValid || typeError != null)
            return;

        var workout = new Workout
        {
            Name = form.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(form.Description) ? null : form.Description.Trim(),
            WorkoutType = selectedTypes.ToList(),
            BodyTarget = form.BodyTarget,
            Equipment = form.Equipment,
            Duration = form.Duration ?? 0,
            Intensity = form.Intensity,
            UserCreated = true
        };

        Service.Add(workout);

        Nav.NavigateTo("/workouts");
    }
}