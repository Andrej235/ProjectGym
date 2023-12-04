using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Delete;
using AppProjectGym.Services.Read;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Pages
{
    public partial class FullScreenExercise : ContentPage, IQueryAttributable
    {
        private readonly IReadService readService;
        private readonly IDeleteService deleteService;

        public FullScreenExercise(IReadService readService, IDeleteService deleteService)
        {
            InitializeComponent();
            BindingContext = this;
            this.readService = readService;
            this.deleteService = deleteService;
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
            Exercise = await readService.Get<Exercise>("none", $"exercise/{id}");

            var images = await readService.Get<List<Image>>("none", ReadService.TranslateEndPoint("image", 0, 1), $"exercise={Exercise.Id}");
            if (images != null && images.Count != 0)
                MainImage = images.First();

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



        private async void OnPrimaryMuscleGroupSearch(object sender, SelectionChangedEventArgs e) => await NavigationService.SearchAsync($"primarymusclegroup={(e.CurrentSelection[0] as MuscleGroupDisplay).Id}");

        private async void OnPrimaryMuscleSearch(object sender, SelectionChangedEventArgs e) => await NavigationService.SearchAsync($"primarymuscle={(e.CurrentSelection[0] as Muscle).Id}");

        private async void OnEquipmentSearch(object sender, SelectionChangedEventArgs e) => await NavigationService.SearchAsync($"equipment={(e.CurrentSelection[0] as Equipment).Id}");

        private async void OnEditButtonClicked(object sender, EventArgs e) => await NavigationService.GoToAsync(nameof(ExerciseCreationPage), new KeyValuePair<string, object>("edit", Exercise));

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            await deleteService.Delete(Exercise);
            await NavigationService.GoToAsync("..");
        }
    }
}