using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Read;
using System.Diagnostics;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Pages
{
    public partial class FullScreenExercise : ContentPage, IQueryAttributable
    {
        private readonly IReadService readService;

        public FullScreenExercise(IReadService readService)
        {
            InitializeComponent();
            BindingContext = this;
            this.readService = readService;
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

        public Image MainImage
        {
            get => mainImage;
            set
            {
                mainImage = value;
                OnPropertyChanged();
            }
        }
        private Image mainImage;

        public List<MuscleGroup> PrimaryMuscleGroups
        {
            get => primaryMuscleGroups;
            set
            {
                primaryMuscleGroups = value;
                OnPropertyChanged();
            }
        }
        private List<MuscleGroup> primaryMuscleGroups;

        public List<MuscleGroup> SecondaryMuscleGroups
        {
            get => secondaryMuscleGroups;
            set
            {
                secondaryMuscleGroups = value;
                OnPropertyChanged();
            }
        }
        private List<MuscleGroup> secondaryMuscleGroups;

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

        public List<ExerciseNote> Notes
        {
            get => notes;
            set
            {
                notes = value;
                OnPropertyChanged();
            }
        }
        private List<ExerciseNote> notes;



        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            int id = Convert.ToInt32(query["id"]);
            Exercise = await readService.Get<Exercise>("all", $"exercise/{id}");

            if (exercise.ImageIds.Any())
            {
                var image = await readService.Get<Image>("all", $"image/{exercise.ImageIds.First()}");
                MainImage = image;
            }

            PrimaryMuscleGroups = await readService.Get<List<MuscleGroup>>("none", "musclegroup", $"primary={Exercise.Id}");
            SecondaryMuscleGroups = await readService.Get<List<MuscleGroup>>("none", "musclegroup", $"secondary={Exercise.Id}");
            PrimaryMuscles = await readService.Get<List<Muscle>>("none", "muscle", $"primary={Exercise.Id}");
            SecondaryMuscles = await readService.Get<List<Muscle>>("none", "muscle", $"secondary={Exercise.Id}");
            Notes = await readService.Get<List<ExerciseNote>>("none", "note", $"exercise={Exercise.Id}");
            Equipment = await readService.Get<List<Equipment>>("none", "equipment", $"exercise={Exercise.Id}");
        }



        private async void OnPrimaryMuscleSearch(object sender, SelectionChangedEventArgs e)
        {
            var muscle = e.CurrentSelection[0] as Muscle;
            Dictionary<string, object> navigationParameter = new()
            {
                {"q", $"primarymuscle={muscle.Id}"}
            };

            await Shell.Current.GoToAsync(nameof(SearchResultsPage), navigationParameter);
        }

        private async void OnSecondaryMuscleSearch(object sender, SelectionChangedEventArgs e)
        {
            var muscle = e.CurrentSelection[0] as Muscle;
            Dictionary<string, object> navigationParameter = new()
            {
                {"q", $"secondarymuscle={muscle.Id}"}
            };

            await Shell.Current.GoToAsync(nameof(SearchResultsPage), navigationParameter);
        }
    }
}