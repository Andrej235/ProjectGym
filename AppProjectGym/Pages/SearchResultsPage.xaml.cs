using AppProjectGym.Models;
using AppProjectGym.Services;
using System.Diagnostics;

namespace AppProjectGym.Pages;

public partial class SearchResultsPage : ContentPage, IQueryAttributable
{
    public SearchResultsPage(IExerciseSearchData exerciseSearchData)
    {
        InitializeComponent();
        BindingContext = this;

        this.exerciseSearchData = exerciseSearchData;
    }
    private readonly IExerciseSearchData exerciseSearchData;

    public List<Exercise> Exercises
    {
        get => exercises;
        set
        {
            exercises = value;
            exerciseDisplays = exercises.Select(e => new ExerciseDisplay()
            {
                Id = e.Id,
                Name = e.Name,
                ImageUrl = e.Images.FirstOrDefault(i => i.IsMain)?.ImageURL ?? ""
            }).ToList();
            OnPropertyChanged();
        }
    }
    private List<Exercise> exercises;
    private List<ExerciseDisplay> exerciseDisplays;


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
        var exercisesToLoad = await exerciseSearchData.Search(searchQuery, "images", (pageNumber - 1) * exercisesPerPage, exercisesPerPage);
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

        var exercise = exercises.FirstOrDefault(e => e.Id == selectedExerciseDisplay.Id);
        if (exercise is null)
            return;

        Dictionary<string, object> navigationParameter = new()
        {
            {nameof(Exercise), exercise}
        };

        await Shell.Current.GoToAsync(nameof(FullScreenExercise), navigationParameter);
    }
}