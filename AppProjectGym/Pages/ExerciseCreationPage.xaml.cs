using AppProjectGym.Models;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Read;
using System.Collections.Generic;
using System.Diagnostics;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Pages;

public partial class ExerciseCreationPage : ContentPage
{
    private readonly IReadService readService;
    private readonly ICreateService createService;

    public ExerciseCreationPage(IReadService readService, ICreateService createService)
    {
        InitializeComponent();
        BindingContext = this;

        this.readService = readService;
        this.createService = createService;
        OnOpen();
    }

    private async void OnOpen()
    {
        Muscles = await readService.Get<List<Muscle>>();
        Equipment = await readService.Get<List<Equipment>>();
    }

    public List<Muscle> Muscles
    {
        get => muscles;
        set
        {
            muscles = value;
            OnPropertyChanged();
        }
    }
    private List<Muscle> muscles;

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

    private async void OnCreateExercise(object sender, EventArgs e)
    {
        var name = nameInput.Text;
        var description = descriptionInput.Text;
        var imageURL = imageURLInput.Text;

        if (name is null ||
            name == "" ||
            description is null ||
            description == "" ||
            imageURL is null ||
            imageURL == "" ||
            equipmentSelector.SelectedItems is null ||
            primaryMuscleSelector.SelectedItems is null)
            return;

        Exercise exercise = new()
        {
            Name = name,
            Description = description,
            EquipmentIds = equipmentSelector.SelectedItems.Select(x => (x as Equipment).Id),
            PrimaryMuscleIds = primaryMuscleSelector.SelectedItems.Select(x => (x as Muscle).Id),
            SecondaryMuscleIds = secondaryMuscleSelector.SelectedItems is not null ? secondaryMuscleSelector.SelectedItems.Select(x => (x as Muscle).Id) : new List<int>(),
            AliasIds = new List<int>(),
            ImageIds = new List<int>(),
            NoteIds = new List<int>(),
        };

        var exerciseId = await createService.Add(exercise);
        if (exerciseId == "")
            return;

        await createService.Add(new Image()
        {
            ImageURL = imageURL,
            ExerciseId = int.Parse(exerciseId),
        });
    }
}