using AppProjectGym.Information;
using AppProjectGym.LocalDatabase;
using AppProjectGym.LocalDatabase.Models;
using AppProjectGym.Models;
using AppProjectGym.Services.Mapping;
using AppProjectGym.Services.Read;

namespace AppProjectGym.Pages;

public partial class ProfilePage : ContentPage
{
    private User user;
    private readonly FinishedWorkoutContext context;
    private readonly IReadService readService;
    private readonly IEntityDisplayMapper<WorkoutSet, StartedWorkout_SetDisplay> startedWorkoutSetDisplayMapper;

    public ProfilePage(IReadService readService, IEntityDisplayMapper<WorkoutSet, StartedWorkout_SetDisplay> startedWorkoutSetDisplayMapper)
    {
        InitializeComponent();

        context = FinishedWorkoutContext.Context;
        BindingContext = this;
        User = ClientInfo.User;
        this.readService = readService;
        this.startedWorkoutSetDisplayMapper = startedWorkoutSetDisplayMapper;
    }

    public User User
    {
        get => user;
        private set
        {
            user = value;
            OnPropertyChanged();
        }
    }

    public List<FinishedWorkout> FinishedWorkouts
    {
        get => finishedWorkouts;
        set
        {
            finishedWorkouts = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(FinishedWorkoutsCount));
        }
    }
    private List<FinishedWorkout> finishedWorkouts;
    public int FinishedWorkoutsCount => FinishedWorkouts is null ? 0 : FinishedWorkouts.Count;



    public FinishedWorkout SelectedWorkout
    {
        get => selectedWorkout;
        set
        {
            selectedWorkout = value;
            OnPropertyChanged();
        }
    }
    private FinishedWorkout selectedWorkout;

    public List<StartedWorkout_SetDisplay> FinishedSets
    {
        get => finishedSets;
        set
        {
            finishedSets = value;
            OnPropertyChanged();
        }
    }
    private List<StartedWorkout_SetDisplay> finishedSets;


    protected override void OnAppearing()
    {
        FinishedWorkouts = [.. context.FinishedWorkouts];

        base.OnAppearing();
    }

    private async void OnOpenFinishedWorkout(object sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not FinishedWorkout selectedWorkout)
            return;

        finishedWorkoutDisplayWrapper.IsVisible = true;
        SelectedWorkout = selectedWorkout;

        FinishedSets =
        [
            .. (await Task.WhenAll(selectedWorkout.WorkoutSets.Select(async x =>
                    {
                        WorkoutSet workoutSet = await readService.Get<WorkoutSet>("all", $"workoutset/{x.WorkoutSetId}");
                        StartedWorkout_SetDisplay startedWorkoutSetDisplay = await startedWorkoutSetDisplayMapper.Map(workoutSet);
                        startedWorkoutSetDisplay.FinishedSets = x.Sets.Select(y => new FinishedSetDisplay()
                        {
                            FinishedReps = y.Reps,
                            Time = y.Time,
                            RestTime = y.RestTime,
                            Weight = new() { Weight = y.Weight }
                        }).ToList();

                        return startedWorkoutSetDisplay;
                    })))
        ];

        finishedSetsCollection.ItemsSource = null;
        finishedSetsCollection.ItemsSource = FinishedSets;
    }
}