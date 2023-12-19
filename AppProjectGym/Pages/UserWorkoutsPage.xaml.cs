using AppProjectGym.Information;
using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Read;
using AppProjectGym.Utilities;

namespace AppProjectGym.Pages;

public partial class UserWorkoutsPage : ContentPage
{
    private delegate void StartHandler();
    private StartHandler startHandler;

    private readonly IReadService readService;

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

    public UserWorkoutsPage(IReadService readService)
    {
        InitializeComponent();
        this.readService = readService;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        Workouts = await readService.Get<List<Workout>>("none", "workout", "personal=true", $"user={ClientInfo.User.Id}");
        base.OnAppearing();
    }

    //Start dialog
    private void OnCancelClicked(object sender, EventArgs e) => CloseStartDialog();

    private void OnYesClicked(object sender, EventArgs e)
    {
        try
        {
            startHandler();
            CloseStartDialog();
        }
        catch (Exception ex)
        {
            LogDebugger.LogError(ex);
        }
    }

    private void CloseStartDialog()
    {
        startDialogWrapper.IsVisible = false;
        whiteOverlay.IsVisible = false;
    }

    private void OpenConfirmDialog(Workout workout)
    {
        startDialogMessage.Text = $"Are you sure you want to start \"{workout.Name}\" workout?";
        startDialogWrapper.IsVisible = true;
        whiteOverlay.IsVisible = true;
    }

    private void OnWhiteOverlayClicked(object sender, EventArgs e) => CloseStartDialog();

    private void OnWorkoutClicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not Workout workout)
            return;

        startHandler = async () =>
        {
            await NavigationService.GoToAsync(nameof(StartedWorkoutPage), new KeyValuePair<string, object>("workout", workout));
        };
        OpenConfirmDialog(workout);
    }
}