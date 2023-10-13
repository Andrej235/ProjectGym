using System.Diagnostics;
using System.Net.Http;
using System;
using System.Text.Json;
using AppProjectGym.Pages;
using AppProjectGym.Models;
using AppProjectGym.Services;

namespace AppProjectGym
{
    public partial class MainPage : ContentPage
    {
        private readonly IDataService<Exercise> exerciseDataService;
        private readonly JsonSerializerOptions serializerOptions;
        private static List<Exercise> exercises;
        public static List<Exercise> Exercises { get => exercises; set => exercises = value; }

        public MainPage(IDataService<Exercise> dataService)
        {
            InitializeComponent();

            serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            BindingContext = this;
            exerciseDataService = dataService;
            LoadExercises();
        }

        private async void LoadExercises()
        {
            Exercises = await exerciseDataService.Get();
            exerciseCollectionView.ItemsSource = Exercises;
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
            var searchBar = sender as SearchBar;
            var searchQuery = searchBar.Text;
            Dictionary<string, object> navigationParameter = new()
            {
                {"q", $"name={searchQuery}"}
            };

            searchBar.Text = "";
            await Shell.Current.GoToAsync(nameof(SearchResultsPage), navigationParameter);
        }
    }
}