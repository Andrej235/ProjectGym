using System.Diagnostics;
using System.Text.Json;
using AppProjectGym.Pages;
using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Information;
using AppProjectGym.Services.Read;

namespace AppProjectGym
{
    public partial class MainPage : ContentPage
    {
        private readonly IReadService readService;
        private readonly ExerciseDisplayMapper exerciseDisplayMapper;

        public static List<Exercise> Exercises { get => exercises; set => exercises = value; }
        private static List<Exercise> exercises;

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
        private bool isWaitingForExerciseData;

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

        private List<ExerciseDisplay> exerciseDisplays;

        public MainPage(
            ExerciseDisplayMapper exerciseDisplayMapper,
            IReadService readService)
        {
            InitializeComponent();

            BindingContext = this;
            this.readService = readService;
            this.exerciseDisplayMapper = exerciseDisplayMapper;

            PageNumber = 1;
            exercisesPerPage = 15;

            LoadExercises();
            LoadMuscles();
            LoadEquipment();
            CheckIfLoggedIn();
        }

        private static async void CheckIfLoggedIn()
        {
            if (!await ClientInfo.SetUser())
            {
                Debug.WriteLine("---> Open login page");
                await NavigationService.GoToAsync(nameof(LoginPage));
            }

            Debug.WriteLine($"---> Client guid: {ClientInfo.ClientGuid}");
        }

        private bool areFiltersOpen = false;
        private bool isPlayingFilterAnimation = false;

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

            Exercises = loadedExercises;
            var newExerciseDisplays = new List<ExerciseDisplay>();
            foreach (var e in Exercises)
                newExerciseDisplays.Add(await exerciseDisplayMapper.Map(e));

            exerciseDisplays = [.. newExerciseDisplays.OrderByDescending(x => x.ImageUrl != "")];
            exerciseCollectionView.ItemsSource = null;
            exerciseCollectionView.ItemsSource = exerciseDisplays;
            await exerciseCollectionScrollView.ScrollToAsync(0, 0, true);
            isWaitingForExerciseData = false;
        }

        private async void LoadMuscles() => Muscles = await readService.Get<List<Muscle>>();

        private async void LoadEquipment() => Equipment = await readService.Get<List<Equipment>>();

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (exerciseCollectionView.ItemsSource is not null)
                exerciseCollectionView.SelectedItem = null;
        }

        private async void OnExerciseSelect(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection is null || !e.CurrentSelection.Any())
                return;

            var exercise = e.CurrentSelection[0] as ExerciseDisplay;
            Debug.WriteLine($"---> Selected {exercise.Name}");

            await NavigationService.GoToAsync(nameof(FullScreenExercise), new KeyValuePair<string, object>("id", exercise.Id));
        }

        private async void OnSearch(object sender, EventArgs e)
        {
            List<Muscle> primaryMusclesSelected = primaryMuscleFilter.SelectedItems.Where(x => x is Muscle).Cast<Muscle>().ToList();
            List<Muscle> secondaryMusclesSelected = secondaryMuscleFilter.SelectedItems.Where(x => x is Muscle).Cast<Muscle>().ToList();
            List<Equipment> equipmentSelected = equipmentFilter.SelectedItems.Where(x => x is Equipment).Cast<Equipment>().ToList();

            string nameQ = searchBar.Text == string.Empty ? string.Empty : $"name={searchBar.Text};";
            string primaryMusclesQ = $"primarymuscle={string.Join(',', primaryMusclesSelected.Select(m => m.Id))};";
            string secondaryMusclesQ = $"secondarymuscle={string.Join(',', secondaryMusclesSelected.Select(m => m.Id))};";
            string equipmentQ = $"equipment={string.Join(',', equipmentSelected.Select(eq => eq.Id))};";

            searchBar.Text = "";
            await NavigationService.SearchAsync(nameQ, primaryMusclesQ, secondaryMusclesQ, equipmentQ, "strict=false");
        }

        private async void FiltersButtonClicked(object sender, EventArgs e)
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
            primaryMuscleFilter.SelectedItems = null;
            secondaryMuscleFilter.SelectedItems = null;
            equipmentFilter.SelectedItems = null;
        }

        private async void OnOpenProfilePage(object sender, EventArgs e)
        {
            await NavigationService.GoToAsync(nameof(ProfilePage));
        }



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

        private async void OnAddButtonPressed(object sender, EventArgs e) => await NavigationService.GoToAsync(nameof(ExerciseCreationPage));
    }
}