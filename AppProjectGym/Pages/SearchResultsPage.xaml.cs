using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Read;
using System.Diagnostics;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Pages;

public partial class SearchResultsPage : ContentPage, IQueryAttributable
{
    public SearchResultsPage(IReadService<Image> imageReadService, IReadService<Exercise> exerciseReadService)
    {
        InitializeComponent();
        BindingContext = this;

        this.imageReadService = imageReadService;
        this.exerciseReadService = exerciseReadService;
    }
    private readonly IReadService<Image> imageReadService;
    private readonly IReadService<Exercise> exerciseReadService;

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
        {
            var imgUrl = "";
            try
            {
                var id = e.ImageIds.FirstOrDefault();
                if (id != 0)
                    imgUrl = (await imageReadService.Get(id.ToString(), "none")).ImageURL;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message}");
            }

            newExerciseDisplays.Add(new()
            {
                Id = e.Id,
                Name = e.Name,
                ImageUrl = imgUrl
            });
        }
        exerciseDisplays = newExerciseDisplays;
        OnPropertyChanged();
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
        exercisesCollection.ItemsSource = null;
        exercisesCollection.ItemsSource = exerciseDisplays;
        await scrollView.ScrollToAsync(0, 0, true);
        isWaitingForData = false;
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
        Debug.WriteLine($"---> Selected {selectedExerciseDisplay.Id}");

        var exercise = await exerciseReadService.Get(selectedExerciseDisplay.Id.ToString(), "none");// exercises.FirstOrDefault(e => e.Id == selectedExerciseDisplay.Id);
        if (exercise is null)
            return;

        Dictionary<string, object> navigationParameter = new()
        {
            {nameof(Exercise), exercise}
        };

        await Shell.Current.GoToAsync(nameof(FullScreenExercise), navigationParameter);
    }
}