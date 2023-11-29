using AppProjectGym.Models;
using AppProjectGym.Services.Read;
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

        public List<MuscleGroupDisplay> PrimaryMuscleDisplays
        {
            get => primaryMuscleDisplays;
            set
            {
                primaryMuscleDisplays = value;
                OnPropertyChanged();
            }
        }
        private List<MuscleGroupDisplay> primaryMuscleDisplays;

        public List<MuscleGroupDisplay> SecondaryMuscleDisplays
        {
            get => secondaryMuscleDisplays;
            set
            {
                secondaryMuscleDisplays = value;
                OnPropertyChanged();
            }
        }
        private List<MuscleGroupDisplay> secondaryMuscleDisplays;

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

            var primaryMuscleGroups = await readService.Get<List<MuscleGroup>>("none", "musclegroup", $"primary={Exercise.Id}");
            var primaryMuscles = await readService.Get<List<Muscle>>("none", "muscle", $"primary={Exercise.Id}");
            PrimaryMuscleDisplays = primaryMuscleGroups.Select(x => new MuscleGroupDisplay()
            {
                Id = x.Id,
                Name = x.Name,
                Muscles = primaryMuscles.Where(y => y.MuscleGroupId == x.Id)
            }).ToList();

            var secondaryMuscleGroups = await readService.Get<List<MuscleGroup>>("none", "musclegroup", $"secondary={Exercise.Id}");
            var secondaryMuscles = await readService.Get<List<Muscle>>("none", "muscle", $"secondary={Exercise.Id}");
            SecondaryMuscleDisplays = secondaryMuscleGroups.Select(x => new MuscleGroupDisplay()
            {
                Id = x.Id,
                Name = x.Name,
                Muscles = secondaryMuscles.Where(y => y.MuscleGroupId == x.Id)
            }).ToList();

            Notes = await readService.Get<List<ExerciseNote>>("none", "note", $"exercise={Exercise.Id}");
            Equipment = await readService.Get<List<Equipment>>("none", "equipment", $"exercise={Exercise.Id}");
        }



        private async void OnPrimaryMuscleGroupSearch(object sender, SelectionChangedEventArgs e)
        {
            var muscle = e.CurrentSelection[0] as MuscleGroupDisplay;
            Dictionary<string, object> navigationParameter = new()
            {
                {"q", $"primarymusclegroup={muscle.Id}"}
            };

            await Shell.Current.GoToAsync(nameof(SearchResultsPage), navigationParameter);
        }
    }
}