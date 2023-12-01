using AppProjectGym.Models;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Delete;
using AppProjectGym.Services.Read;
using AppProjectGym.Services.Update;
using System.Collections.Generic;
using System.Diagnostics;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Pages;

public partial class ExerciseCreationPage : ContentPage, IQueryAttributable
{
    private readonly IReadService readService;
    private readonly ICreateService createService;
    private readonly IUpdateService updateService;
    private readonly IDeleteService deleteService;
    private bool isEditing;

    public ExerciseCreationPage(IReadService readService, ICreateService createService, IUpdateService updateService, IDeleteService deleteService)
    {
        InitializeComponent();
        BindingContext = this;

        this.readService = readService;
        this.createService = createService;
        this.updateService = updateService;
        this.deleteService = deleteService;
    }

    private List<MuscleGroup> muscleGroups;
    private List<Muscle> muscles;

    public List<MuscleGroupDisplay> MuscleGroupDisplays
    {
        get => muscleGroupDisplays;
        set
        {
            muscleGroupDisplays = value;
            OnPropertyChanged();
        }
    }
    private List<MuscleGroupDisplay> muscleGroupDisplays;

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

    private Exercise exercise;

    private async void OnCreateExercise(object sender, EventArgs e)
    {
        if (isEditing)
        {
            await UpdateExercise();
            return;
        }

        var name = nameInput.Text;
        var description = descriptionInput.Text;
        var imageURL = imageURLInput.Text;

        if (name is null ||
            name == "" ||
            description is null ||
            description == "" ||
            equipmentSelector.SelectedItems is null ||
            primaryMuscleGroupSelector.SelectedItems is null)
            return;

        Exercise exercise = new()
        {
            Name = name,
            Description = description,
            EquipmentIds = equipmentSelector.SelectedItems.Select(x => (x as Equipment).Id),
            PrimaryMuscleIds = primaryMuscleGroupSelector.SelectedItems.Select(x => (x as Muscle).Id),
            SecondaryMuscleIds = secondaryMuscleGroupSelector.SelectedItems is not null ? secondaryMuscleGroupSelector.SelectedItems.Select(x => (x as Muscle).Id) : new List<int>(),
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

    public async Task UpdateExercise()
    {
        await updateService.Update(exercise);
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        muscleGroups = await readService.Get<List<MuscleGroup>>("all");
        muscles = await readService.Get<List<Muscle>>("all");
        Equipment = await readService.Get<List<Equipment>>("all");

        MuscleGroupDisplays = muscleGroups.Select(x => new MuscleGroupDisplay()
        {
            Id = x.Id,
            Name = x.Name,
            Muscles = muscles.Where(y => y.MuscleGroupId == x.Id)
        }).ToList();

        if (!query.TryGetValue("edit", out object value))
        {
            isEditing = false;
            return;
        }

        isEditing = true;
        exercise = value as Exercise;
        nameInput.Text = exercise.Name;
        descriptionInput.Text = exercise.Description;
        imageURLInput.Text = "Not implemented";

        equipmentSelector.SelectedItems = Equipment.Where(x => x.UsedInExerciseIds.Contains(exercise.Id)).Cast<object>().ToList();
        //TODO: Add default selected primary and secondary muscles (and groups), selecting a muscle should select it's group as well

        primaryMuscleGroupSelector.SelectedItems = MuscleGroupDisplays.Where(x => muscleGroups.First(y => y.Id == x.Id).PrimaryInExercises.Contains(exercise.Id)).Cast<object>().ToList();
        secondaryMuscleGroupSelector.SelectedItems = MuscleGroupDisplays.Where(x => muscleGroups.First(y => y.Id == x.Id).SecondaryInExercises.Contains(exercise.Id)).Cast<object>().ToList();
        
        //primaryMuscleSelector.SelectedItems = Muscles.Where(x => x.PrimaryInExercises.Contains(exercise.Id)).Cast<object>().ToList();
        //secondaryMuscleSelector.SelectedItems = Muscles.Where(x => x.SecondaryInExercises.Contains(exercise.Id)).Cast<object>().ToList();
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}