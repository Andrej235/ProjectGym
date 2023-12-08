using AppProjectGym.Models;
using AppProjectGym.Services.Read;

namespace AppProjectGym.Pages;

public partial class MuscleCreationPage : ContentPage
{
    private readonly IReadService readService;

    public MuscleCreationPage(IReadService readService)
    {
        InitializeComponent();
        BindingContext = this;
        this.readService = readService;
    }

    private List<MuscleGroup> muscleGroups;
    private List<Muscle> muscles;
    private List<MuscleGroupDisplay> muscleGroupRepresentations;
    public List<MuscleGroupDisplay> MuscleGroupRepresentations
    {
        get => muscleGroupRepresentations;
        set
        {
            muscleGroupRepresentations = value;
            OnPropertyChanged();
        }
    }



    protected override async void OnAppearing()
    {
        base.OnAppearing();

        muscleGroups = await readService.Get<List<MuscleGroup>>();
        muscles = await readService.Get<List<Muscle>>();

        MuscleGroupRepresentations = muscleGroups.Select(x => new MuscleGroupDisplay()
        {
            Id = x.Id,
            Name = x.Name,
            Muscles = muscles.Where(y => y.MuscleGroupId == x.Id)
        }).ToList();
    }
}