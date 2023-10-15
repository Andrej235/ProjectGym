using System.Diagnostics;
using System.Text.Json;
using AppProjectGym.Pages;
using AppProjectGym.Models;
using AppProjectGym.Services;

namespace AppProjectGym
{
    public partial class MainPage : ContentPage
    {
        private readonly IDataService<Exercise> exerciseDataService;
        private readonly IDataService<Muscle> muscleDataService;
        private readonly JsonSerializerOptions serializerOptions;
        public static List<Exercise> Exercises { get => exercises; set => exercises = value; }
        private static List<Exercise> exercises;

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



        public MainPage(
            IDataService<Exercise> exerciseDataService,
            IDataService<Muscle> muscleDataService
            )
        {
            InitializeComponent();

            serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            BindingContext = this;
            this.exerciseDataService = exerciseDataService;
            this.muscleDataService = muscleDataService;
            LoadExercises();
            LoadMuscles();
        }

        private bool areFiltersOpen = false;
        private bool isPlayingFilterAnimation = false;

        private async void LoadExercises()
        {
            Exercises = await exerciseDataService.Get();
            exerciseCollectionView.ItemsSource = Exercises;
        }

        private async void LoadMuscles()
        {
            Muscles = await muscleDataService.Get();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (exerciseCollectionView.ItemsSource is not null)
                exerciseCollectionView.SelectedItem = null;
        }

        private void OnExerciseClicked(object sender, EventArgs e)
        {
            if ((sender as ImageButton).BindingContext is not Exercise exercise)
                return;

            Debug.WriteLine(exercise.Name);
        }

        private async void OnExerciseSelect(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection is null || !e.CurrentSelection.Any())
                return;

            var exercise = e.CurrentSelection[0] as Exercise;
            Debug.WriteLine($"---> Selected {exercise.Name}");

            Dictionary<string, object> navigationParameter = new()
            {
                {nameof(Exercise), exercise }
            };

            await Shell.Current.GoToAsync(nameof(FullScreenExercise), navigationParameter);
        }

        private async void OnSearch(object sender, EventArgs e)
        {
            List<Muscle> primaryMusclesSelected = primaryMuscleFilter.SelectedItems.Where(x => x is Muscle).Cast<Muscle>().ToList();
            List<Muscle> secondaryMusclesSelected = secondaryMuscleFilter.SelectedItems.Where(x => x is Muscle).Cast<Muscle>().ToList();

            string nameQ = searchBar.Text == string.Empty ? string.Empty : $"name={searchBar.Text};";
            string primaryMusclesQ = $"primarymuscle={string.Join(',', primaryMusclesSelected.Select(m => m.Id))};";
            string secondaryMusclesQ = $"secondarymuscle={string.Join(',', secondaryMusclesSelected.Select(m => m.Id))};";

            Dictionary<string, object> navigationParameter = new()
            {
                {"q", $"{nameQ}{primaryMusclesQ}{secondaryMusclesQ};strict=false"}
            };

            searchBar.Text = "";
            await Shell.Current.GoToAsync(nameof(SearchResultsPage), navigationParameter);
        }

        private async void FiltersButtonClicked(object sender, EventArgs e)
        {
            if (isPlayingFilterAnimation)
                return;

            isPlayingFilterAnimation = true;
            await filtersWrapper.ScaleYTo(areFiltersOpen ? 0 : 1);
            areFiltersOpen = !areFiltersOpen;
            isPlayingFilterAnimation = false;
        }
    }
}