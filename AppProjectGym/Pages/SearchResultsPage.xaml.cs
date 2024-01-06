using AppProjectGym.Information;
using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Mapping;
using AppProjectGym.Services.Read;
using System.Diagnostics;

namespace AppProjectGym.Pages
{
    public partial class SearchResultsPage : ContentPage, IQueryAttributable
    {
        private readonly IReadService readService;
        private readonly IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper;

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
        private readonly int exercisesPerPage;
        private string searchQuery;
        private bool bookmarkedExercisesOnly;
        private bool isWaitingForData;

        private bool isInSelectionMode;

        #region On Load
        public SearchResultsPage(IReadService readService, IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper)
        {
            InitializeComponent();
            BindingContext = this;

            this.readService = readService;
            this.exerciseDisplayMapper = exerciseDisplayMapper;

            PageNumber = 1;
            exercisesPerPage = 10;
            LoadFilters();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            searchQuery = query["q"] as string;
            bookmarkedExercisesOnly = searchQuery.Contains("bookmarked");

            LoadExercises();
            isInSelectionMode = query.TryGetValue("selectionMode", out object selectionModeObj) && selectionModeObj is bool selectionMode && selectionMode;
        }

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

            Exercises = !bookmarkedExercisesOnly ? exercisesToLoad : exercisesToLoad.Where(x => ClientInfo.User.BookmarkIds.Contains(x.Id)).ToList();
            await scrollView.ScrollToAsync(0, 0, true);
            isWaitingForData = false;
        }

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

            primaryMuscleFilter.ItemsSource = null;
            primaryMuscleFilter.ItemsSource = PrimaryMuscleGroupRepresentations;
            secondaryMuscleFilter.ItemsSource = null;
            secondaryMuscleFilter.ItemsSource = SecondaryMuscleGroupRepresentations;
        }
        #endregion

        #region Exercise selection / lazy loading
        private async void OnExerciseSelect(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection is null || !e.CurrentSelection.Any())
                return;

            var selectedExerciseDisplay = e.CurrentSelection[0] as ExerciseDisplay;
            Debug.WriteLine($"---> Selected {selectedExerciseDisplay.Exercise.Name}");

            await NavigationService.GoToAsync(nameof(FullScreenExercisePage), new KeyValuePair<string, object>("id", selectedExerciseDisplay.Exercise.Id), new KeyValuePair<string, object>("selectionMode", isInSelectionMode));
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
        #endregion

        #region Filters / Search
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



        private void OnSearch(object sender, EventArgs e)
        {
            if (isWaitingForData)
                return;

            string searchText = searchBar.Text;
            searchBar.Text = "";

            IEnumerable<int> equipmentIds = equipmentFilter.SelectedItems.Cast<Equipment>().Select(x => x.Id);

            IEnumerable<int> primaryMuscleGroupIds = primaryMuscleFilter.SelectedItems.Cast<MuscleGroupRepresentation>().Select(x => x.MuscleGroupDisplay.Id);
            IEnumerable<int> primaryMuscleIds = PrimaryMuscleGroupRepresentations.SelectMany(x => x.SelectedMuscles).Select(x => x.Id);

            IEnumerable<int> secondaryMuscleGroupIds = SecondaryMuscleGroupRepresentations.Where(x => x.SelectedMuscles.Count > 0).Select(x => x.MuscleGroupDisplay.Id);
            IEnumerable<int> secondaryMuscleIds = SecondaryMuscleGroupRepresentations.SelectMany(x => x.SelectedMuscles).Select(x => x.Id);

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

            if (bookmarksCheckBoxFilter.IsChecked)
                queryPairs.Add($"bookmarked");

            searchQuery = queryPairs.Count > 1 || !queryPairs.Any(x => x.Contains("strict=")) ? string.Join(';', queryPairs) : "";
            bookmarkedExercisesOnly = searchQuery.Contains("bookmarked");

            LoadExercises();
            filtersWrapper.IsVisible = false;
        }

        private void FiltersButtonClicked(object sender, EventArgs e) => filtersWrapper.IsVisible = !filtersWrapper.IsVisible;

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

            bookmarksCheckBoxFilter.IsChecked = false;

            equipmentFilterExpander.IsExpanded = false;
            primaryMuscleFilterExpander.IsExpanded = false;
            secondaryMuscleFilterExpander.IsExpanded = false;
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

        private void OnBookmarkFilterButtonClicked(object sender, EventArgs e) => bookmarksCheckBoxFilter.IsChecked = !bookmarksCheckBoxFilter.IsChecked;
        #endregion
    }
}