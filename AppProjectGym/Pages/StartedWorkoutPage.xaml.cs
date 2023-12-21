using AppProjectGym.Information;
using AppProjectGym.Models;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Mapping;
using AppProjectGym.Services.Read;
using AppProjectGym.Utilities;

namespace AppProjectGym.Pages;

public partial class StartedWorkoutPage : ContentPage, IQueryAttributable
{
    private delegate void EditWeightHandler(float weightDifference);
    private EditWeightHandler editWeightHandler;

    private delegate Task CreateNewWeightHandler(float newWeight);
    private CreateNewWeightHandler createNewWeightHandler;

    private readonly IReadService readService;
    private readonly ICreateService createService;
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



    public StartedWorkoutPage(IReadService readService, IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper, IEntityDisplayMapper<WorkoutSet, WorkoutSetDisplay> workoutSetDisplayMapper, ICreateService createService)
    {
        InitializeComponent();
        this.readService = readService;
        this.createService = createService;
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

        RefreshSetCollection();
        SelectedWorkoutSet = null;
    }

    private void RefreshSetCollection()
    {
        setCollection.ItemsSource = null;
        setCollection.ItemsSource = WorkoutSetDisplays;
    }

    private void OnSetSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection[0] is not WorkoutSetDisplay workoutSetDisplay)
            return;

        SelectedWorkoutSet = workoutSetDisplay;
    }

    private void OnWeightClicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not SetDisplay setDisplay)
            return;

        editWeightHandler = weightDif =>
        {
            var newWeight = setDisplay.Weight.Weight + weightDif;
            if (newWeight < 0)
                return;

            weightEditorEntry.Text = newWeight.ToString();
            setDisplay.Weight.Weight = newWeight;
        };

        createNewWeightHandler = async weight =>
        {
            PersonalExerciseWeight newWeight = new()
            {
                Weight = weight,
                ExerciseId = setDisplay.Exercise.Exercise.Id,
                UserId = ClientInfo.User.Id,
            };

            if (await createService.Add(newWeight, "weight") == default)
                return;

            setDisplay.Weight = newWeight;
            RefreshSetCollection();
        };

        OpenWeighEditorDialog();
    }

    private void OnWeightEdited(object sender, EventArgs e)
    {
        if (sender is not Button button || !float.TryParse(button.Text, out float weightDif))
            return;

        editWeightHandler(weightDif);
    }

    private void OpenWeighEditorDialog(float currentWeight = 0)
    {
        weightEditorDialogWrapper.IsVisible = true;
        weightEditorEntry.Text = currentWeight.ToString();
        whiteOverlay.IsVisible = true;
    }

    private void CloseWeighEditorDialog()
    {
        weightEditorDialogWrapper.IsVisible = false;
        whiteOverlay.IsVisible = false;
    }

    private void OnWhiteOverlayClicked(object sender, EventArgs e) => CloseWeighEditorDialog();

    private async void OnWeightCreate(object sender, EventArgs e)
    {
        if (!float.TryParse(weightEditorEntry.Text, out float newWeight))
            return;

        try
        {
            await createNewWeightHandler(newWeight);
            CloseWeighEditorDialog();
        }
        catch (Exception ex)
        {
            LogDebugger.LogError(ex);
        }
    }
}