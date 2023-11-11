using AppProjectGym.Models;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Read;
using System.Diagnostics;

namespace AppProjectGym.Pages;

public partial class ExerciseCreationPage : ContentPage
{
    private readonly IReadService<Muscle> muscleReadService;
    private readonly IReadService<ExerciseCategory> categoryReadService;
    private readonly IReadService<Equipment> equipmentReadService;
    private readonly ICreateService<Exercise> createService;

    public ExerciseCreationPage(IReadService<Muscle> muscleReadService, IReadService<ExerciseCategory> categoryReadService, IReadService<Equipment> equipmentReadService, ICreateService<Exercise> createService)
    {
        InitializeComponent();
        BindingContext = this;

        this.muscleReadService = muscleReadService;
        this.categoryReadService = categoryReadService;
        this.equipmentReadService = equipmentReadService;

        OnOpen();
        this.createService = createService;
    }

    private async void OnOpen()
    {
        Muscles = await muscleReadService.Get("", 0, -1, "none");
        Equipment = await equipmentReadService.Get("", 0, -1, "none");
        Categories = await categoryReadService.Get("", 0, -1, "none");
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

    public List<ExerciseCategory> Categories
    {
        get => categories;
        set
        {
            categories = value;
            OnPropertyChanged();
        }
    }
    private List<ExerciseCategory> categories;

    private void OnCreateExercise(object sender, EventArgs e)
    {
        var name = nameInput.Text;
        var description = descriptionInput.Text;

        if (name is null ||
            name == "" ||
            description is null ||
            description == "" ||
            categorySelector.SelectedItem is null ||
            equipmentSelector.SelectedItems is null ||
            primaryMuscleSelector.SelectedItems is null)
            return;

        Exercise exercise = new()
        {
            Name = name,
            Description = description,
            CategoryId = (categorySelector.SelectedItem as ExerciseCategory).Id,
            EquipmentIds = equipmentSelector.SelectedItems.Select(x => (x as Equipment).Id),
            PrimaryMuscleIds = primaryMuscleSelector.SelectedItems.Select(x => (x as Muscle).Id),
            SecondaryMuscleIds = secondaryMuscleSelector.SelectedItems is not null ? secondaryMuscleSelector.SelectedItems.Select(x => (x as Muscle).Id) : new List<int>(),
            AliasIds = new List<int>(),
            ImageIds = new List<int>(),
            VideoIds = new List<int>(),
            IsVariationOfIds = new List<int>(),
            VariationIds = new List<int>(),
            NoteIds = new List<int>(),//ctrl k d
        };
        createService.Add(exercise);
    }
}