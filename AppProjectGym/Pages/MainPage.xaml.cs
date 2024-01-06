using System.Diagnostics;
using AppProjectGym.Pages;
using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Read;
using AppProjectGym.Services.Mapping;
using AppProjectGym.Information;

namespace AppProjectGym
{
    public partial class MainPage : ContentPage
    {
        private readonly IReadService readService;
        private readonly IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper;
        private readonly int exercisesPerPage;

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
        private bool isWaitingForExerciseData;

        private List<ExerciseDisplay> exerciseDisplays;



        #region On Load
        public MainPage(IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper, IReadService readService)
        {
            InitializeComponent();
            BindingContext = this;

            this.readService = readService;
            this.exerciseDisplayMapper = exerciseDisplayMapper;

            PageNumber = 1;
            exercisesPerPage = 10;
            OnLoad();
        }

        private async void OnLoad()
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

        private async void LoadExercises()
        {
            if (isWaitingForExerciseData)
                return;

            isWaitingForExerciseData = true;
            var loadedExercises = await readService.Get<List<Exercise>>("images", ReadService.TranslateEndPoint("exercise", (pageNumber - 1) * exercisesPerPage, exercisesPerPage));
            if (loadedExercises is null || loadedExercises.Count == 0)
            {
                PageNumber--; //If the page number is 1 the previous button can't even be pressed / won't invoke this method
                isWaitingForExerciseData = false;
                return;
            }

            exerciseDisplays = [.. ((await Task.WhenAll(loadedExercises.Select(async e => await exerciseDisplayMapper.Map(e)))).OrderByDescending(x => x.Image.ImageURL != ""))];

            exerciseCollectionView.ItemsSource = null;
            exerciseCollectionView.ItemsSource = exerciseDisplays;
            exerciseCollectionView.SelectedItem = null;

            await exerciseCollectionScrollView.ScrollToAsync(0, 0, false);
            isWaitingForExerciseData = false;
        }

        protected override void OnAppearing()
        {
            if (areCreateOptionsOpen)
                ToggleCreateOptions();

            LoadExercises();
            base.OnAppearing();
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



        private async void OnSearch(object sender, EventArgs e)
        {
            string searchText = searchBar.Text;

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

            searchBar.Text = "";
            await NavigationService.SearchAsync([.. queryPairs]);
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

        #region Navigation bar (bottom 3 buttons)
        private async void OnOpenProfilePage(object sender, EventArgs e)
        {
            if (!ClientInfo.IsLoadingData)
                await NavigationService.GoToAsync(nameof(ProfilePage));
        }

        private async void OnOpenUserWorkoutsPage(object sender, EventArgs e)
        {
            if (!ClientInfo.IsLoadingData)
                await NavigationService.GoToAsync(nameof(UserWorkoutsPage));
        }

        #region Create options
        private bool areCreateOptionsOpen = false;
        private bool isPlayingCreateOptionsAnimation = false;
        private void OnAddButtonPressed(object sender, EventArgs e) => ToggleCreateOptions();

        private void ToggleCreateOptions()
        {
            if (isPlayingCreateOptionsAnimation)
                return;

            isPlayingCreateOptionsAnimation = true;

            if (areCreateOptionsOpen)
            {
                createOptionsWrapper.ScaleY = 0;
                createOptionsWrapper.IsVisible = false;
            }
            else
            {
                createOptionsWrapper.IsVisible = true;
                createOptionsWrapper.ScaleY = 1;
            }

            areCreateOptionsOpen = !areCreateOptionsOpen;
            isPlayingCreateOptionsAnimation = false;
        }


        private async void OnExerciseCreateClicked(object sender, EventArgs e) => await NavigationService.GoToAsync(nameof(ExerciseCreationPage));

        private async void OnMuscleCreateClicked(object sender, EventArgs e) => await NavigationService.GoToAsync(nameof(MuscleCreationPage));

        private async void OnEquipmentCreateClicked(object sender, EventArgs e) => await NavigationService.GoToAsync(nameof(EquipmentCreationPage));

        private async void OnWorkoutCreateClicked(object sender, EventArgs e) => await NavigationService.GoToAsync(nameof(WorkoutCreationPage));
        #endregion

        #endregion

        #region Exercise selection / lazy loading
        private void LoadPreviousPage(object sender, EventArgs e)
        {
            if (PageNumber > 1 && !isWaitingForExerciseData)
            {
                PageNumber--;
                LoadExercises();
            }
        }

        private void LoadNextPage(object sender, EventArgs e)
        {
            if (!isWaitingForExerciseData)
            {
                PageNumber++;
                LoadExercises();
            }
        }

        private async void OnExerciseSelect(object sender, SelectionChangedEventArgs e)
        {
            if (isWaitingForExerciseData || e.CurrentSelection is null || !e.CurrentSelection.Any())
                return;

            var exerciseDisplay = e.CurrentSelection[0] as ExerciseDisplay;
            Debug.WriteLine($"---> Selected {exerciseDisplay.Exercise.Name}");

            await NavigationService.GoToAsync(nameof(FullScreenExercisePage), new KeyValuePair<string, object>("id", exerciseDisplay.Exercise.Id));
        }
        #endregion

        protected override bool OnBackButtonPressed()
        {
            if (PageNumber > 1 && !isWaitingForExerciseData)
            {
                PageNumber--;
                LoadExercises();
                return true;
            }

            return false;
        }
    }
}