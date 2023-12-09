using AppProjectGym.Models;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Delete;
using AppProjectGym.Services.Read;
using AppProjectGym.Services.Update;
using AppProjectGym.Utilities;

namespace AppProjectGym.Pages;

public partial class EquipmentCreationPage : ContentPage
{
    private delegate Task<bool> InputHandler(string input);
    private delegate Task ConfirmHandler(bool input);

    private InputHandler inputHandler;
    private ConfirmHandler confirmHandler;

    private readonly ICreateService createService;
    private readonly IReadService readService;
    private readonly IUpdateService updateService;
    private readonly IDeleteService deleteService;

    public EquipmentCreationPage(ICreateService createService, IReadService readService, IUpdateService updateService, IDeleteService deleteService)
    {
        InitializeComponent();
        BindingContext = this;

        this.createService = createService;
        this.readService = readService;
        this.updateService = updateService;
        this.deleteService = deleteService;
    }

    public List<Equipment> Equipment
    {
        get => equipment;
        set
        {
            equipment = value;
            OnPropertyChanged();
        }
    }
    private List<Equipment> equipment;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Equipment = await readService.Get<List<Equipment>>();
    }

    private void OnEquipmentCreate(object sender, EventArgs e)
    {
        Equipment equipment = new();
        inputHandler = async s =>
        {
            equipment.Name = s;

            var idString = await createService.Add(equipment);
            if (idString == default || !int.TryParse(idString, out int id))
                return false;

            equipment.Id = id;
            Equipment.Add(equipment);

            equipmentCollection.ItemsSource = null;
            equipmentCollection.ItemsSource = Equipment;
            return true;
        };

        OpenInputDialog();
    }

    private void OnEquipmentEdit(object sender, EventArgs e)
    {
        if (sender is not ImageButton imgButton)
            return;

        Equipment equipment = imgButton.BindingContext as Equipment;
        inputHandler = async s =>
        {
            if (s is null || s == "" || s == equipment.Name)
                return false;

            equipment.Name = s;
            var success = await updateService.Update(equipment);

            if (!success)
                return false;

            Equipment.First(x => x.Id == equipment.Id).Name = s;

            equipmentCollection.ItemsSource = null;
            equipmentCollection.ItemsSource = Equipment;
            return true;
        };

        OpenInputDialog(equipment.Name);
    }

    private void OnEquipmentDelete(object sender, EventArgs e)
    {
        if (sender is not ImageButton imgButton)
            return;

        Equipment equipment = imgButton.BindingContext as Equipment;
        confirmHandler = async choice =>
        {
            if (!choice)
                return;

            var success = await deleteService.Delete(equipment);
            if (!success)
                return;

            Equipment.Remove(equipment);

            equipmentCollection.ItemsSource = null;
            equipmentCollection.ItemsSource = Equipment;
        };

        OpenConfirmDialog($"Are you sure you want to delete {equipment.Name}?");
    }

    private void OnWhiteOverlayClicked(object sender, EventArgs e)
    {
        CloseInputDialog();
        CloseConfirmDialog();
    }

    private void OnCancelClicked(object sender, EventArgs e) => HandleConfirmDialog(false);
    private void OnYesClicked(object sender, EventArgs e) => HandleConfirmDialog(true);
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



    private void OnInputSubmit(object sender, EventArgs e) => HandleInput(equipmentNameInput.Text);

    private async void HandleInput(string input)
    {
        try
        {
            var success = await inputHandler(input);
            if (success)
                CloseInputDialog();

            equipmentNameInput.Text = "";
            equipmentNameInput.Placeholder = "Something went wrong";
            return;
        }
        catch (Exception ex)
        {
            LogDebugger.LogError(ex);
            equipmentNameInput.Text = "";
            equipmentNameInput.Placeholder = "Something went wrong";
        }
    }

    private void OpenInputDialog()
    {
        equipmentNameInput.Placeholder = "Enter equipment name: ";
        nameInputDialogWrapper.IsVisible = true;
        whiteOverlay.IsVisible = true;
    }

    private void OpenInputDialog(string defaultValue)
    {
        equipmentNameInput.Text = defaultValue;
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
}