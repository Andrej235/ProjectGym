using AppProjectGym.Models;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Read;
using AppProjectGym.Services.Update;
using AppProjectGym.Services.Delete;
using AppProjectGym.Utilities;
using AppProjectGym.Information;
using AppProjectGym.Services;

namespace AppProjectGym.Pages;

public partial class WorkoutCreationPage : ContentPage
{
    public List<Workout> Workouts
    {
        get => workouts;
        set
        {
            workouts = value;
            OnPropertyChanged();
        }
    }
    private List<Workout> workouts;

    private readonly ICreateService createService;
    private readonly IReadService readService;
    private readonly IUpdateService updateService;
    private readonly IDeleteService deleteService;

    public WorkoutCreationPage(ICreateService createService, IReadService readService, IUpdateService updateService, IDeleteService deleteService)
    {
        InitializeComponent();
        BindingContext = this;

        this.createService = createService;
        this.readService = readService;
        this.updateService = updateService;
        this.deleteService = deleteService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Workouts = await readService.Get<List<Workout>>("none", "workout", $"user={ClientInfo.User.Id}", "personal=true");
    }

    #region Input dialogs
    //Input dialogs
    private delegate Task<bool> InputHandler(string input);
    private delegate Task ConfirmHandler(bool input);

    private InputHandler inputHandler;
    private ConfirmHandler confirmHandler;

    private void OpenInputDialog()
    {
        workoutNameInput.Placeholder = "Enter equipment name: ";
        nameInputDialogWrapper.IsVisible = true;
        whiteOverlay.IsVisible = true;
    }

    private void OpenInputDialog(string defaultValue)
    {
        workoutNameInput.Text = defaultValue;
        OpenInputDialog();
    }

    private void OpenConfirmDialog(string message)
    {
        confirmDialogMessage.Text = message;
        confirmDialogWrapper.IsVisible = true;
        whiteOverlay.IsVisible = true;
    }

    private void CloseInputDialog()
    {
        nameInputDialogWrapper.IsVisible = false;
        whiteOverlay.IsVisible = false;
    }

    private void CloseConfirmDialog()
    {
        confirmDialogWrapper.IsVisible = false;
        whiteOverlay.IsVisible = false;
    }

    private void OnWhiteOverlayClicked(object sender, EventArgs e)
    {
        CloseInputDialog();
        CloseConfirmDialog();
    }

    //Handles
    private async void HandleInput(string input)
    {
        try
        {
            var success = await inputHandler(input);
            if (success)
                CloseInputDialog();

            workoutNameInput.Text = "";
            workoutNameInput.Placeholder = "Something went wrong";
            return;
        }
        catch (Exception ex)
        {
            LogDebugger.LogError(ex);
            workoutNameInput.Text = "";
            workoutNameInput.Placeholder = "Something went wrong";
        }
    }
    private async void HandleConfirmDialog(bool choice)
    {
        try
        {
            await confirmHandler(choice);
            CloseConfirmDialog();
        }
        catch (Exception ex)
        {
            LogDebugger.LogError(ex);
        }
    }

    //Submits
    private void OnInputSubmit(object sender, EventArgs e) => HandleInput(workoutNameInput.Text);
    private void OnCancelClicked(object sender, EventArgs e) => HandleConfirmDialog(false);
    private void OnYesClicked(object sender, EventArgs e) => HandleConfirmDialog(true);
    #endregion



    private void OnWorkoutCreate(object sender, EventArgs e)
    {
        Workout workout = new();
        inputHandler = async s =>
        {
            workout.Name = s;
            workout.CreatorId = ClientInfo.User.Id;

            var idString = await createService.Add(workout);
            if (idString == default || !Guid.TryParse(idString.Replace("\"", ""), out Guid id))
                return false;

            workout.Id = id;
            Workouts.Add(workout);

            workoutCollection.ItemsSource = null;
            workoutCollection.ItemsSource = Workouts;
            return true;
        };

        OpenInputDialog();
    }

    private async void OnWorkoutEdit(object sender, EventArgs e)
    {
        if (sender is not ImageButton imgButton)
            return;

        Workout workout = imgButton.BindingContext as Workout;
        await NavigationService.GoToAsync(nameof(WorkoutEditPage), new KeyValuePair<string, object>("workout", workout));
    }

    private void OnWorkoutDelete(object sender, EventArgs e)
    {
        if (sender is not ImageButton imgButton)
            return;

        Workout workout = imgButton.BindingContext as Workout;
        confirmHandler = async choice =>
        {
            if (!choice)
                return;

            var success = await deleteService.Delete(workout);
            if (!success)
                return;

            Workouts.Remove(workout);

            workoutCollection.ItemsSource = null;
            workoutCollection.ItemsSource = Workouts;
        };

        OpenConfirmDialog($"Are you sure you want to delete {workout.Name}?");
    }
}