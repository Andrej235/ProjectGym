using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Read;
using System.Diagnostics;

namespace AppProjectGym.Pages;

public partial class SearchResultsPage : ContentPage, IQueryAttributable
{
    public SearchResultsPage(IReadService readService, ExerciseDisplayMapper exerciseDisplayMapper)
    {
        InitializeComponent();
        BindingContext = this;

        this.readService = readService;
        this.exerciseDisplayMapper = exerciseDisplayMapper;
    }
    private readonly IReadService readService;
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

    private bool isInSelectionMode;

    private async void SetExerciseDisplays()
    {
        List<ExerciseDisplay> newExerciseDisplays = [];
        foreach (var e in exercises)
            newExerciseDisplays.Add(await exerciseDisplayMapper.Map(e));

        exerciseDisplays = [.. newExerciseDisplays.OrderBy(x => x.Image.ImageURL == "")];
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
        LoadFilters();

        isInSelectionMode = query.TryGetValue("selectionMode", out object selectionModeObj) && selectionModeObj is bool selectionMode && selectionMode;
    }

    public async void LoadExercises()
    {
        if (searchQuery is null)
            return;

        if (isWaitingForData)
            return;

        isWaitingForData = true;
        var exercisesToLoad = await readService.Get<List<Exercise>>("images", ReadService.TranslateEndPoint("exercise", (pageNumber - 1) * exercisesPerPage, exercisesPerPage), searchQuery);
        if (exercisesToLoad is null || exercisesToLoad.Count == 0)
        {
            PageNumber--;
            isWaitingForData = false;
            return;
        }

        Exercises = exercisesToLoad;
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
        Debug.WriteLine($"---> Selected {selectedExerciseDisplay.Exercise.Name}");

        await NavigationService.GoToAsync(nameof(FullScreenExercise), new KeyValuePair<string, object>("id", selectedExerciseDisplay.Exercise.Id), new KeyValuePair<string, object>("selectionMode", isInSelectionMode));
    }

    #region Filters
    public List<MuscleGroupRepresentation> PrimaryMuscleGroupRepresentations
    {
        get => primaryMuscleGroupRepresentations;
        set
        {
            primaryMuscleGroupRepresentations = value;
            OnPropertyChanged();
        }
    }
    private List<MuscleGroupRepresentation> primaryMuscleGroupRepresentations;

    public List<MuscleGroupRepresentation> SecondaryMuscleGroupRepresentations
    {
        get => secondaryMuscleGroupRepresentations;
        set
        {
            secondaryMuscleGroupRepresentations = value;
            OnPropertyChanged();
        }
    }
    private List<MuscleGroupRepresentation> secondaryMuscleGroupRepresentations;

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

    private List<MuscleGroupDisplay> muscleGroupDisplays;
    private bool areFiltersOpen = false;
    private bool isPlayingFilterAnimation = false;

    private async void LoadFilters()
    {
        Equipment = await readService.Get<List<Equipment>>();
        var muscles = await readService.Get<List<Muscle>>();
        var muscleGroups = await readService.Get<List<MuscleGroup>>();
        muscleGroupDisplays = muscleGroups.Select(x => new MuscleGroupDisplay()
        {
            Id = x.Id,
            Name = x.Name,
            Muscles = muscles.Where(y => y.MuscleGroupId == x.Id),
        }).ToList();

        PrimaryMuscleGroupRepresentations = muscleGroupDisplays.Select(x => new MuscleGroupRepresentation()
        {
            MuscleGroupDisplay = x,
            SelectedMuscles = []
        }).ToList();

        SecondaryMuscleGroupRepresentations = muscleGroupDisplays.Select(x => new MuscleGroupRepresentation()
        {
            MuscleGroupDisplay = x,
            SelectedMuscles = []
        }).ToList();
    }

    private void OnSearch(object sender, EventArgs e)
    {
        string searchText = searchBar.Text;

        IEnumerable<int> equipmentIds = equipmentFilter.SelectedItems.Cast<Equipment>().Select(x => x.Id);

        IEnumerable<int> primaryMuscleGroupIds = primaryMuscleFilter.SelectedItems.Cast<MuscleGroupRepresentation>().Select(x => x.MuscleGroupDisplay.Id);
        IEnumerable<int> primaryMuscleIds = PrimaryMuscleGroupRepresentations.SelectMany(x => x.SelectedMuscles).Select(x => x.Id);

        IEnumerable<int> secondaryMuscleGroupIds = secondaryMuscleFilter.SelectedItems.Cast<MuscleGroupRepresentation>().Select(x => x.MuscleGroupDisplay.Id);
        IEnumerable<int> secondaryMuscleIds = SecondaryMuscleGroupRepresentations.SelectMany(x => x.SelectedMuscles).Select(x => x.Id);

        searchBar.Text = "";

        List<string> queryPairs = [
            $"name={searchText}",
            $"equipment={string.Join(',', equipmentIds)}",
            $"primarymusclegroup={string.Join(',', primaryMuscleGroupIds)}",
            $"primarymuscle={string.Join(',', primaryMuscleIds)}",
            $"secondarymusclegroup={string.Join(',', secondaryMuscleGroupIds)}",
            $"secondarymuscle={string.Join(',', secondaryMuscleIds)}",
            "strict=false"
            ];
        queryPairs = queryPairs.Where(x => x.Contains('=') && x.Split('=').Where(y => y.Length > 0).Count() == 2).ToList();

        PageNumber = 1;
        exercisesPerPage = 10;
        searchQuery = queryPairs.Count > 1 ? string.Join(';', queryPairs) : "";
        LoadExercises();
        if (areFiltersOpen)
            ToggleFilterWrapper();
    }

    private void FiltersButtonClicked(object sender, EventArgs e) => ToggleFilterWrapper();

    private async void ToggleFilterWrapper()
    {
        if (isPlayingFilterAnimation)
            return;

        isPlayingFilterAnimation = true;

        if (areFiltersOpen)
        {
            await filtersWrapper.ScaleYTo(0);
            filtersWrapper.IsVisible = false;
        }
        else
        {
            filtersWrapper.IsVisible = true;
            await filtersWrapper.ScaleYTo(1);
        }

        areFiltersOpen = !areFiltersOpen;
        isPlayingFilterAnimation = false;
    }

    private void OnClearFilters(object sender, EventArgs e)
    {
        foreach (MuscleGroupRepresentation muscleGroupRepresentation in PrimaryMuscleGroupRepresentations)
            muscleGroupRepresentation.SelectedMuscles = [];

        foreach (MuscleGroupRepresentation muscleGroupRepresentation in SecondaryMuscleGroupRepresentations)
            muscleGroupRepresentation.SelectedMuscles = [];

        primaryMuscleFilter.SelectedItems = null;
        primaryMuscleFilter.ItemsSource = null;
        primaryMuscleFilter.ItemsSource = PrimaryMuscleGroupRepresentations;

        secondaryMuscleFilter.SelectedItems = null;
        secondaryMuscleFilter.ItemsSource = null;
        secondaryMuscleFilter.ItemsSource = SecondaryMuscleGroupRepresentations;

        equipmentFilter.SelectedItems = null;
    }

    private void OnMuscleFilterSelect(object sender, SelectionChangedEventArgs e)
    {
        if (sender is CollectionView innerCollection && innerCollection.ItemsSource is IEnumerable<Muscle> musclesSource)
        {
            if (innerCollection.Parent.Parent.Parent is CollectionView outerCollection)
            {
                MuscleGroupDisplay display = muscleGroupDisplays.First(x => x.Id == musclesSource.First().MuscleGroupId);
                IEnumerable<Muscle> selectedMuscles = innerCollection.SelectedItems.Cast<Muscle>();
                if (outerCollection == primaryMuscleFilter)
                {
                    var muscleGroupRepresentation = PrimaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay == display);
                    var oldSelectedMuscles = muscleGroupRepresentation.SelectedMuscles;
                    muscleGroupRepresentation.SelectedMuscles.AddRange(selectedMuscles.Where(muscle => !oldSelectedMuscles.Contains(muscle)));

                    var musclesToDelete = oldSelectedMuscles.Where(muscle => !selectedMuscles.Contains(muscle)).ToList();
                    for (int i = 0; i < musclesToDelete.Count; i++)
                        PrimaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay == display).SelectedMuscles.Remove(musclesToDelete[i]);

                    if (muscleGroupRepresentation.SelectedMuscles.Count > 0)
                    {
                        if (!outerCollection.SelectedItems.Contains(muscleGroupRepresentation))
                            outerCollection.SelectedItems.Add(muscleGroupRepresentation);
                    }
                    else if (outerCollection.SelectedItems.Contains(muscleGroupRepresentation))
                    {
                        outerCollection.SelectedItems.Remove(muscleGroupRepresentation);
                    }
                }
                else if (outerCollection == secondaryMuscleFilter)
                {
                    var muscleGroupRepresentation = secondaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay == display);
                    var oldSelectedMuscles = muscleGroupRepresentation.SelectedMuscles;
                    muscleGroupRepresentation.SelectedMuscles.AddRange(selectedMuscles.Where(muscle => !oldSelectedMuscles.Contains(muscle)));

                    var musclesToDelete = oldSelectedMuscles.Where(muscle => !selectedMuscles.Contains(muscle)).ToList();
                    for (int i = 0; i < musclesToDelete.Count; i++)
                        secondaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay == display).SelectedMuscles.Remove(musclesToDelete[i]);

                    if (muscleGroupRepresentation.SelectedMuscles.Count > 0)
                    {
                        if (!outerCollection.SelectedItems.Contains(muscleGroupRepresentation))
                            outerCollection.SelectedItems.Add(muscleGroupRepresentation);
                    }
                    else if (outerCollection.SelectedItems.Contains(muscleGroupRepresentation))
                    {
                        outerCollection.SelectedItems.Remove(muscleGroupRepresentation);
                    }
                }
            }
        }
    }
    #endregion
}