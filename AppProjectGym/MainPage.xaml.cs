using System.Diagnostics;
using System.Net.Http;
using System;
using System.Text.Json;

namespace AppProjectGym
{
    public partial class MainPage : ContentPage
    {
        public static double ImageWidth { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("http://192.168.1.9:5054/api/exercise");

                var body = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<List<ExerciseDTO>>(body);
                res = res.Where(x => x.imageUrls.Any()).ToList(); //TODO: Instead of deleting ones without images push them to the end of the list

                int itemsPerRow = 3;
                double screenWidth = DeviceDisplay.MainDisplayInfo.Width;
                ImageWidth = screenWidth / itemsPerRow;
                Debug.WriteLine($"---> Calculated width: {ImageWidth}"); //Bind to min and max height if border of item template

                exerciseCollectionView.ItemsSource = res;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
            }
        }

        public class ExerciseDTO
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string category { get; set; }
            public string[] imageUrls { get; set; }
            public string[] videoUrls { get; set; }
            public int?[] isVariationOf { get; set; }
            public int?[] variations { get; set; }
            public string[] equipmentName { get; set; }
            public string[] primaryMuscles { get; set; }
            public string[] secondaryMuscles { get; set; }
            public string[] aliases { get; set; }
            public string[] notes { get; set; }
        }

    }
}