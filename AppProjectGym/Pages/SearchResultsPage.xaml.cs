using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Read;
using System.Diagnostics;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Pages;

public partial class SearchResultsPage : ContentPage, IQueryAttributable
{
    public SearchResultsPage(IReadService<Image> imageReadService, IReadService<Exercise> exerciseReadService, ExerciseDisplayMapper exerciseDisplayMapper)
    {
        InitializeComponent();
        BindingContext = this;

        this.imageReadService = imageReadService;
        this.exerciseReadService = exerciseReadService;
        this.exerciseDisplayMapper = exerciseDisplayMapper;
    }
    private readonly IReadService<Image> imageReadService;
    private readonly IReadService<Exercise> exerciseReadService;
    private readonly ExerciseDisplayMapper exerciseDisplayMapper;

    public List<Exercise> Exercises
    {
        get => exercises;
        set
        {
            exercises = value;
            SetExerciseDisplays();
        }
    }
    private List<Exercise> exercises;
    private List<ExerciseDisplay> exerciseDisplays;

    private async void SetExerciseDisplays()
    {
        List<ExerciseDisplay> newExerciseDisplays = new();
        foreach (var e in exercises)
            newExerciseDisplays.Add(await exerciseDisplayMapper.Map(e));

        exerciseDisplays = newExerciseDisplays;
        exercisesCollection.ItemsSource = null;
        exercisesCollection.ItemsSource = exerciseDisplays;
        isWaitingForData = false;
    }

    public int PageNumber
    {
        get => pageNumber;
        set
        {
            pageNumber = value;
            pageNumberLabel.Text = null;
            pageNumberLabel.Text = pageNumber.ToString();
        }
    }
    private int pageNumber;
    private int exercisesPerPage;
    private string searchQuery;
    private bool isWaitingForData;
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        PageNumber = 1;
        exercisesPerPage = 10;
        searchQuery = query["q"] as string;
        LoadExercises();
    }

    public async void LoadExercises()
    {
        if (searchQuery is null)
            return;

        if (isWaitingForData)
            return;

        isWaitingForData = true;
        var exercisesToLoad = await exerciseReadService.Get(searchQuery, (pageNumber - 1) * exercisesPerPage, exercisesPerPage, "images");
        if (exercisesToLoad is null || !exercisesToLoad.Any())
        {
            PageNumber--;
            return;
        }

        Exercises = exercisesToLoad;
        await scrollView.ScrollToAsync(0, 0, true);
    }

    private void LoadPreviousPage(object sender, EventArgs e)
    {
        if (PageNumber > 1 && !isWaitingForData)
        {
            PageNumber--;
            LoadExercises();
        }
    }

    private void LoadNextPage(object sender, EventArgs e)
    {
        if (!isWaitingForData)
        {
            PageNumber++;
            LoadExercises();
        }
    }

    private async void OnExerciseSelect(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection is null || !e.CurrentSelection.Any())
            return;

        var selectedExerciseDisplay = e.CurrentSelection[0] as ExerciseDisplay;
        Debug.WriteLine($"---> Selected {selectedExerciseDisplay.Name}");

        Dictionary<string, object> navigationParameter = new()
        {
            {"id", selectedExerciseDisplay.Id}
        };

        await Shell.Current.GoToAsync(nameof(FullScreenExercise), navigationParameter);
    }
}