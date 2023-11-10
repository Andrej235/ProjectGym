using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Read;
using System.Diagnostics;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Pages
{
    public partial class FullScreenExercise : ContentPage, IQueryAttributable
    {
        private readonly IDataService<Muscle> muscleDataService;
        private readonly IDataService<ExerciseCategory> categoryDataService;
        private readonly IDataService<ExerciseNote> notesDataService;
        private readonly IDataService<Equipment> equipmentDataService;

        private readonly IReadService<Exercise> exerciseReadService;
        private readonly IReadService<Image> imageReadService;

        public FullScreenExercise(
            IDataService<Muscle> muscleDataService,
            IDataService<ExerciseCategory> categoryDataService,
            IDataService<ExerciseNote> notesDataService,
            IDataService<Equipment> equipmentDataService,
            IReadService<Exercise> exerciseReadService,
            IReadService<Image> imageReadService)
        {
            InitializeComponent();

            BindingContext = this;
            this.muscleDataService = muscleDataService;
            this.categoryDataService = categoryDataService;
            this.notesDataService = notesDataService;
            this.equipmentDataService = equipmentDataService;
            this.exerciseReadService = exerciseReadService;
            this.imageReadService = imageReadService;
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

        public Models.Image MainImage
        {
            get => mainImage;
            set
            {
                mainImage = value;
                OnPropertyChanged();
            }
        }
        private Models.Image mainImage;

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

        public ExerciseCategory Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged();
            }
        }
        private ExerciseCategory category;

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
            Exercise = await exerciseReadService.Get(id.ToString(), "all");

            if (exercise.ImageIds.Any())
            {
                var image = await imageReadService.Get(exercise.ImageIds.First().ToString(), "all");
                MainImage = image;
            }
            //OnOpen();
        }

        private void OnOpen()
        {
            LoadCategory();
            LoadMuscles();
            LoadNotes();
            LoadEquipment();
        }

        private async void LoadMuscles()
        {
            PrimaryMuscles = (await Task.WhenAll(Exercise.PrimaryMuscleIds
                .Select(muscleDataService.Get))
                .ConfigureAwait(false)).ToList();

            SecondaryMuscles = (await Task.WhenAll(Exercise.SecondaryMuscleIds
                .Select(muscleDataService.Get))
                .ConfigureAwait(false)).ToList();
        }

        private async void LoadCategory() => Category = await categoryDataService.Get(Exercise.CategoryId);

        private async void LoadNotes() => Notes = (await Task.WhenAll(Exercise.NoteIds.Select(notesDataService.Get))).ToList();

        private async void LoadEquipment() => Equipment = (await Task.WhenAll(Exercise.EquipmentIds.Select(equipmentDataService.Get))).ToList();



        private async void OnCategoryClicked(object sender, EventArgs e)
        {
            var category = (sender as Button).BindingContext as ExerciseCategory;
            Dictionary<string, object> navigationParameter = new()
            {
                {"q", $"category={category.Id}"}
            };

            await Shell.Current.GoToAsync(nameof(SearchResultsPage), navigationParameter);
            Debug.WriteLine($"---> Category clicked {category.Name}");
        }

        private async void OnSelectPrimaryMuscle(object sender, SelectionChangedEventArgs e)
        {
            var muscle = e.CurrentSelection[0] as Muscle;
            Dictionary<string, object> navigationParameter = new()
            {
                {"q", $"primarymuscle={muscle.Id}"}
            };

            await Shell.Current.GoToAsync(nameof(SearchResultsPage), navigationParameter);
        }

        private async void OnSelectSecondaryMuscle(object sender, SelectionChangedEventArgs e)
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