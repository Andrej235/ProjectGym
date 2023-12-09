using AppProjectGym.Models;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Delete;
using AppProjectGym.Services.Read;
using AppProjectGym.Services.Update;
using AppProjectGym.Utilities;

namespace AppProjectGym.Pages;

public partial class MuscleCreationPage : ContentPage
{
    private delegate Task<bool> InputHandler(string input);
    private delegate Task ConfirmHandler(bool input);

    private InputHandler inputHandler;
    private ConfirmHandler confirmHandler;

    private readonly ICreateService createService;
    private readonly IReadService readService;
    private readonly IUpdateService updateService;
    private readonly IDeleteService deleteService;

    public MuscleCreationPage(ICreateService createService, IReadService readService, IUpdateService updateService, IDeleteService deleteService)
    {
        InitializeComponent();
        BindingContext = this;
        this.createService = createService;
        this.readService = readService;
        this.updateService = updateService;
        this.deleteService = deleteService;
    }

    private List<MuscleGroup> muscleGroups;
    private List<Muscle> muscles;
    private List<MuscleGroupDisplay> muscleGroupDisplays;
    public List<MuscleGroupDisplay> MuscleGroupDisplays
    {
        get => muscleGroupDisplays;
        set
        {
            muscleGroupDisplays = value;
            OnPropertyChanged();
        }
    }



    protected override async void OnAppearing()
    {
        base.OnAppearing();

        muscleGroups = await readService.Get<List<MuscleGroup>>();
        muscles = await readService.Get<List<Muscle>>();

        MuscleGroupDisplays = muscleGroups.Select(x => new MuscleGroupDisplay()
        {
            Id = x.Id,
            Name = x.Name,
            Muscles = muscles.Where(y => y.MuscleGroupId == x.Id)
        }).ToList();
    }

    private void OnInputSubmit(object sender, EventArgs e) => HandleInput(muscleNameInput.Text);

    private async void HandleInput(string input)
    {
        try
        {
            var success = await inputHandler(input);
            if (success)
                CloseInputDialog();

            muscleNameInput.Text = "";
            muscleNameInput.Placeholder = "Something went wrong";
            return;
        }
        catch (Exception ex)
        {
            LogDebugger.LogError(ex);
            muscleNameInput.Text = "";
            muscleNameInput.Placeholder = "Something went wrong";
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

    private void OnEditMuscleGroupName(object sender, EventArgs e)
    {
        if (sender is not ImageButton imgButton)
            return;

        MuscleGroupDisplay muscleGroupDisplay = imgButton.BindingContext as MuscleGroupDisplay;
        MuscleGroup muscleGroup = new()
        {
            Id = muscleGroupDisplay.Id,
            Name = muscleGroupDisplay.Name,
            MuscleIds = muscleGroupDisplay.Muscles.Select(x => x.Id)
        };

        inputHandler = async s =>
        {
            if (s is null || s == "" || s == muscleGroup.Name)
                return false;

            muscleGroup.Name = s;
            var success = await updateService.Update(muscleGroup);

            if (!success)
                return false;

            MuscleGroupDisplays.First(x => x.Id == muscleGroup.Id).Name = s;
            muscleCollection.ItemsSource = null;
            muscleCollection.ItemsSource = MuscleGroupDisplays;
            return true;
        };

        OpenInputDialog(muscleGroup.Name);
    }

    private void OnEditMuscleName(object sender, EventArgs e)
    {
        if (sender is not ImageButton imgButton)
            return;

        Muscle muscle = imgButton.BindingContext as Muscle;
        inputHandler = async s =>
        {
            if (s is null || s == "" || s == muscle.Name)
                return false;

            muscle.Name = s;
            var success = await updateService.Update(muscle);

            if (!success)
                return false;

            var muscleGroupDisplay = MuscleGroupDisplays.First(x => x.Id == muscle.MuscleGroupId);
            muscleGroupDisplay.Muscles.First(x => x.Id == muscle.Id).Name = s;
            muscleGroupDisplay.Muscles = muscleGroupDisplay.Muscles;

            muscleCollection.ItemsSource = null;
            muscleCollection.ItemsSource = MuscleGroupDisplays;
            return true;
        };

        OpenInputDialog(muscle.Name);
    }

    private void OnMuscleDeleted(object sender, EventArgs e)
    {
        if (sender is not ImageButton imgButton)
            return;

        Muscle muscle = imgButton.BindingContext as Muscle;

        confirmHandler = async choice =>
        {
            if (!choice)
                return;

            var success = await deleteService.Delete(muscle);
            if (!success)
                return;

            var muscleGroupDisplay = MuscleGroupDisplays.First(x => x.Id == muscle.MuscleGroupId);
            var musclesInGroup = muscleGroupDisplay.Muscles.ToList();
            musclesInGroup.Remove(muscle);
            muscleGroupDisplay.Muscles = musclesInGroup;

            muscleCollection.ItemsSource = null;
            muscleCollection.ItemsSource = MuscleGroupDisplays;
        };

        OpenConfirmDialog($"Are you sure you want to delete {muscle.Name}?");
    }

    private void OnMuscleGroupDelete(object sender, EventArgs e)
    {
        if (sender is not ImageButton imgButton)
            return;

        MuscleGroupDisplay muscleGroupDisplay = imgButton.BindingContext as MuscleGroupDisplay;
        MuscleGroup muscleGroup = muscleGroups.First(x => x.Id == muscleGroupDisplay.Id);

        confirmHandler = async choice =>
        {
            if (!choice)
                return;

            var success = await deleteService.Delete(muscleGroup);
            if (!success)
                return;

            MuscleGroupDisplays.Remove(muscleGroupDisplay);

            muscleCollection.ItemsSource = null;
            muscleCollection.ItemsSource = MuscleGroupDisplays;
        };

        OpenConfirmDialog($"Are you sure you want to delete {muscleGroupDisplay.Name}?");
    }

    private void OnMuscleGroupCreate(object sender, EventArgs e)
    {
        MuscleGroup muscleGroup = new();
        inputHandler = async s =>
        {
            muscleGroup.Name = s;
            muscleGroup.MuscleIds = [];

            var idString = await createService.Add(muscleGroup);
            if (idString == default || !int.TryParse(idString, out int id))
                return false;

            muscleGroup.Id = id;
            muscleGroups.Add(muscleGroup);
            MuscleGroupDisplays.Add(new()
            {
                Id = id,
                Name = s,
                Muscles = [],
            });

            muscleCollection.ItemsSource = null;
            muscleCollection.ItemsSource = MuscleGroupDisplays;
            return true;
        };

        OpenInputDialog();
    }

    private void OnMuscleCreate(object sender, EventArgs e)
    {
        int muscleGroupId = Convert.ToInt32((sender as ImageButton).BindingContext);
        if (muscleGroupId == 0)
            return;

        Muscle muscle = new();
        inputHandler = async s =>
        {
            muscle.Name = s;
            muscle.MuscleGroupId = muscleGroupId;

            var idString = await createService.Add(muscle);
            if (idString == default || !int.TryParse(idString, out int id))
                return false;

            muscle.Id = id;
            var muscleGroupDisplay = MuscleGroupDisplays.First(x => x.Id == muscleGroupId);
            var musclesInGroup = muscleGroupDisplay.Muscles.ToList();
            musclesInGroup.Add(muscle);
            muscleGroupDisplay.Muscles = musclesInGroup;

            muscleCollection.ItemsSource = null;
            muscleCollection.ItemsSource = MuscleGroupDisplays;
            return true;
        };

        OpenInputDialog();
    }

    private void OpenInputDialog()
    {
        muscleNameInput.Placeholder = "Enter muscle name: ";
        nameInputDialogWrapper.IsVisible = true;
        whiteOverlay.IsVisible = true;
    }

    private void OpenInputDialog(string defaultValue)
    {
        muscleNameInput.Text = defaultValue;
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

    private void OnCancelClicked(object sender, EventArgs e) => HandleConfirmDialog(false);

    private void OnYesClicked(object sender, EventArgs e) => HandleConfirmDialog(true);
}