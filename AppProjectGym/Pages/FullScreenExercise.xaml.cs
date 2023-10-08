using AppProjectGym.Models;
using AppProjectGym.Services;
using System.Diagnostics;

namespace AppProjectGym.Pages
{
    public partial class FullScreenExercise : ContentPage, IQueryAttributable
    {
        private readonly IDataService<Muscle> muscleDataService;
        private readonly IDataService<Exercise> exerciseDataService;

        public FullScreenExercise(IDataService<Exercise> exerciseDataService, IDataService<Muscle> muscleDataService)
        {
            InitializeComponent();

            BindingContext = this;
            this.exerciseDataService = exerciseDataService;
            this.muscleDataService = muscleDataService;
        }



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

        public List<Muscle> PrimaryMuscles
        {
            get => primaryMuscles;
            set
            {
                primaryMuscles = value;
                OnPropertyChanged();
            }
        }
        private List<Muscle> primaryMuscles;

        public List<Muscle> SecondaryMuscles
        {
            get => secondaryMuscles;
            set
            {
                secondaryMuscles = value;
                OnPropertyChanged();
            }
        }
        private List<Muscle> secondaryMuscles;



        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Debug.WriteLine("---> ApplyQueryAttributes");
            Exercise = query[nameof(Models.Exercise)] as Exercise;
            MainImage = exercise.Images.FirstOrDefault(i => i.IsMain);
            OnOpen();
        }

        private async void OnOpen()
        {
            Exercise = await exerciseDataService.Get(exercise.Id);

            PrimaryMuscles = (await Task.WhenAll(exercise.PrimaryMuscleIds
                .Select(muscleId => muscleDataService.Get(muscleId)))
                .ConfigureAwait(false)).ToList();

            SecondaryMuscles = (await Task.WhenAll(exercise.SecondaryMuscleIds
                .Select(muscleId => muscleDataService.Get(muscleId)))
                .ConfigureAwait(false)).ToList();
        }
    }
}