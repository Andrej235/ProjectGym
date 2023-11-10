using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Read;
using System.Diagnostics;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Pages
{
    public partial class FullScreenExercise : ContentPage, IQueryAttributable
    {
        private readonly IReadService<Exercise> exerciseReadService;
        private readonly IReadService<Image> imageReadService;
        private readonly IReadService<Muscle> muscleReadService;
        private readonly IReadService<ExerciseCategory> categoryReadService;
        private readonly IReadService<ExerciseNote> notesReadService;
        private readonly IReadService<Equipment> equipmentReadService;

        public FullScreenExercise(IReadService<Exercise> exerciseReadService,
                                  IReadService<Image> imageReadService,
                                  IReadService<Muscle> muscleReadService,
                                  IReadService<ExerciseCategory> categoryReadService,
                                  IReadService<ExerciseNote> notesReadService,
                                  IReadService<Equipment> equipmentReadService)
        {
            InitializeComponent();
            BindingContext = this;
            this.exerciseReadService = exerciseReadService;
            this.imageReadService = imageReadService;
            this.muscleReadService = muscleReadService;
            this.categoryReadService = categoryReadService;
            this.notesReadService = notesReadService;
            this.equipmentReadService = equipmentReadService;
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
            OnOpen();
        }

        private void OnOpen()
        {
            LoadPrimaryMuscles();
            LoadSecondaryMuscles();
            LoadCategory();
            LoadNotes();
            LoadEquipment();
        }

        private async void LoadPrimaryMuscles() => PrimaryMuscles = (await Task.WhenAll(Exercise.PrimaryMuscleIds.Select(x => muscleReadService.Get(x.ToString(), "none"))).ConfigureAwait(false)).ToList();

        private async void LoadSecondaryMuscles() => SecondaryMuscles = (await Task.WhenAll(Exercise.SecondaryMuscleIds.Select(x => muscleReadService.Get(x.ToString(), "none"))).ConfigureAwait(false)).ToList();

        private async void LoadCategory() => Category = await categoryReadService.Get(Exercise.CategoryId.ToString(), "none");

        private async void LoadNotes() => Notes = (await Task.WhenAll(Exercise.NoteIds.Select(x => notesReadService.Get(x.ToString(), "none")))).ToList();

        private async void LoadEquipment() => Equipment = (await Task.WhenAll(Exercise.EquipmentIds.Select(x => equipmentReadService.Get(x.ToString(), "none")))).ToList();



        private async void OnCategorySearch(object sender, EventArgs e)
        {
            var category = (sender as Button).BindingContext as ExerciseCategory;
            Dictionary<string, object> navigationParameter = new()
            {
                {"q", $"category={category.Id}"}
            };

            await Shell.Current.GoToAsync(nameof(SearchResultsPage), navigationParameter);
            Debug.WriteLine($"---> Category clicked {category.Name}");
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