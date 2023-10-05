using AppProjectGym.Models;
using System.Diagnostics;

namespace AppProjectGym.Pages
{
    public partial class FullScreenExercise : ContentPage, IQueryAttributable
    {
        private Exercise exercise;
        public Exercise Exercise
        {
            get => exercise;
            set
            {
                exercise = value;
                OnPropertyChanged();
            }
        }

        private ExerciseImage mainImage;
        public ExerciseImage MainImage
        {
            get => mainImage;
            set
            {
                mainImage = value;
                OnPropertyChanged();
            }
        }

        public double ImageWidth { get; private set; }
        public double MaximumImageHeight { get; private set; }


        public FullScreenExercise()
        {
            InitializeComponent();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Debug.WriteLine("---> ApplyQueryAttributes");
            Exercise = query[nameof(Models.Exercise)] as Exercise;
            MainImage = exercise.Images.FirstOrDefault(i => i.IsMain);
            ImageWidth = DeviceDisplay.MainDisplayInfo.Width * 0.75;
            MaximumImageHeight = DeviceDisplay.MainDisplayInfo.Height * 0.5;

            Debug.WriteLine($"---> Image width for full screen: {ImageWidth}");
        }
    }
}