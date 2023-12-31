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


    protected override void OnAppearing()
    {
        FinishedWorkouts = [.. context.FinishedWorkouts];

        base.OnAppearing();
    }

    private async void OnOpenFinishedWorkout(object sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not FinishedWorkout selectedWorkout)
            return;

        //This way is just straight up confusing, none the less it is missing a lot of info on completed individual sets so it needs some more work
        var a = await Task.WhenAll(selectedWorkout.WorkoutSets.Select(async x =>
        {
            WorkoutSet workoutSet = await readService.Get<WorkoutSet>("all", $"workoutset/{x.WorkoutSetId}");
            return await startedWorkoutSetDisplayMapper.Map(workoutSet);
            /*            return new StartedWorkout_SetDisplay()
                        {

                            FinishedSets = x.Sets.Select(y => new FinishedSetDisplay()
                            {
                                FinishedReps = y.Reps,
                                Time = y.Time,
                                Weight = new() { Weight = y.Weight } //Idk if this will work, if I only display the XKG then yea it will
                            }).ToList(),

                            WorkoutSet = new()
                            {
                                Id = x.WorkoutSetId,
                                Set = new()
                                {

                                }
                            }
                        };
            */
        }));

        //finishedWorkoutDisplayWrapper.BindingContext = selectedWorkout;
        finishedWorkoutDisplayWrapper.IsVisible = true;
    }
}