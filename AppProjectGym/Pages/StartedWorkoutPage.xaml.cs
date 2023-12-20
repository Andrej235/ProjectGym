
using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Mapping;
using AppProjectGym.Services.Read;
using System.Diagnostics;

namespace AppProjectGym.Pages;

public partial class StartedWorkoutPage : ContentPage, IQueryAttributable
{
    private readonly IReadService readService;
    private readonly IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper;
    private readonly IEntityDisplayMapper<WorkoutSet, WorkoutSetDisplay> workoutSetDisplayMapper;

    public List<WorkoutSetDisplay> WorkoutSetDisplays
    {
        get => workoutSetDisplays;
        set
        {
            workoutSetDisplays = value;
            OnPropertyChanged();
        }
    }
    private List<WorkoutSetDisplay> workoutSetDisplays;

    public Workout Workout
    {
        get => workout;
        set
        {
            workout = value;
            OnPropertyChanged();
        }
    }
    private Workout workout;

    public WorkoutSetDisplay SelectedWorkoutSet
    {
        get => selectedWorkoutSet;
        set
        {
            selectedWorkoutSet = value;
            OnPropertyChanged();
        }
    }
    private WorkoutSetDisplay selectedWorkoutSet;



    public StartedWorkoutPage(IReadService readService, IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper, IEntityDisplayMapper<WorkoutSet, WorkoutSetDisplay> workoutSetDisplayMapper)
    {
        InitializeComponent();
        this.readService = readService;
        BindingContext = this;
        this.exerciseDisplayMapper = exerciseDisplayMapper;
        this.workoutSetDisplayMapper = workoutSetDisplayMapper;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("workout", out object workoutObj) || workoutObj is not Workout workout)
            return;

        Workout = workout;
        WorkoutSetDisplays = [];

        var workoutSets = await readService.Get<List<WorkoutSet>>("none", "workoutset", $"workout={workout.Id}");
        WorkoutSetDisplays.AddRange(await Task.WhenAll(workoutSets.Select(async workoutSet => await workoutSetDisplayMapper.Map(workoutSet))));

        setCollection.ItemsSource = null;
        setCollection.ItemsSource = WorkoutSetDisplays;
        SelectedWorkoutSet = null;
    }

    private void OnSetSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection[0] is not WorkoutSetDisplay workoutSetDisplay)
            return;

        SelectedWorkoutSet = workoutSetDisplay;
    }
}