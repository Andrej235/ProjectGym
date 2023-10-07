using AppProjectGym.Models;
using AppProjectGym.Services;
using System.Diagnostics;

namespace AppProjectGym.Pages
{
    public partial class FullScreenExercise : ContentPage, IQueryAttributable
    {
        public Exercise Exercise
        {
            get => exercise;
            set
            {
                exercise = value;
                OnPropertyChanged();
            }
        }
        private Exercise exercise;

        public ExerciseImage MainImage
        {
            get => mainImage;
            set
            {
                mainImage = value;
                OnPropertyChanged();
            }
        }
        private ExerciseImage mainImage;

        private readonly IDataService<Muscle> muscleDataService;
        private readonly IDataService<Exercise> exerciseDataService;

        public double ImageWidth { get; private set; }
        public double MaximumImageHeight { get; private set; }


        public FullScreenExercise(IDataService<Exercise> exerciseDataService, IDataService<Muscle> muscleDataService)
        {
            InitializeComponent();

            BindingContext = this;
            this.exerciseDataService = exerciseDataService;
            this.muscleDataService = muscleDataService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Debug.WriteLine("---> ApplyQueryAttributes");
            Exercise = query[nameof(Models.Exercise)] as Exercise;
            MainImage = exercise.Images.FirstOrDefault(i => i.IsMain);
            ImageWidth = DeviceDisplay.MainDisplayInfo.Width * 0.75;
            MaximumImageHeight = DeviceDisplay.MainDisplayInfo.Height * 0.5;
            OnOpen();
        }

        private async void OnOpen()
        {
            Exercise = await exerciseDataService.Get(exercise.Id);

            /*            List<Muscle> primaryMuscles = new();
                        foreach (var muscleId in exercise.PrimaryMuscleIds)
                            primaryMuscles.Add(await muscleDataService.Get(muscleId));*/

            List<Muscle> primaryMuscles = (await Task.WhenAll(exercise.PrimaryMuscleIds
                .Select(muscleId => muscleDataService.Get(muscleId)))
                .ConfigureAwait(false)).ToList();

            List<Muscle> secondaryMuscles = (await Task.WhenAll(exercise.SecondaryMuscleIds
                .Select(muscleId => muscleDataService.Get(muscleId)))
                .ConfigureAwait(false)).ToList();

            foreach (var m in primaryMuscles)
            {
                Debug.WriteLine($"---> Primary: {m.Name}");
            }
        }
    }
}