using System.Diagnostics;
using System.Net.Http;
using System;
using System.Text.Json;
using AppProjectGym.Pages;
using AppProjectGym.Models;

namespace AppProjectGym
{
    public partial class MainPage : ContentPage
    {
        private readonly JsonSerializerOptions serializerOptions;

        private static List<Exercise> exercises;
        public static List<Exercise> Exercises { get => exercises; set => exercises = value; }

        public MainPage()
        {
            InitializeComponent();

            serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            BindingContext = this;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("http://192.168.1.9:5054/api/exercise/basic");

                var body = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<List<Exercise>>(body, serializerOptions);
                res = res.Where(x => x.Images.Any()).ToList(); //TODO: Instead of deleting ones without images push them to the end of the list

                Exercises = res;
                exerciseCollectionView.ItemsSource = res;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
            }
        }

        private void OnExerciseClicked(object sender, EventArgs e)
        {
            if ((sender as ImageButton).BindingContext is not Exercise exercise)
                return;

            Debug.WriteLine(exercise.Name);
        }

        private async void OnExerciseSelect(object sender, SelectionChangedEventArgs e)
        {
            var exercise = (e.CurrentSelection[0] as Exercise);
            Debug.WriteLine($"---> Selected {exercise}");

            Dictionary<string, object> navigationParameter = new()
            {
                {nameof(Exercise), exercise }
            };

            await Shell.Current.GoToAsync(nameof(FullScreenExercise), navigationParameter);
        }
    }
}