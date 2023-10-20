using AppProjectGym.Models;
using AppProjectGym.Services;
using System.Diagnostics;

namespace AppProjectGym.Pages
{
    public partial class FullScreenExercise : ContentPage, IQueryAttributable
    {
        private readonly IDataService<Muscle> muscleDataService;
        private readonly IDataService<Exercise> exerciseDataService;
        private readonly IDataService<ExerciseCategory> categoryDataService;
        private readonly IDataService<ExerciseNote> notesDataService;
        private readonly IDataService<Equipment> equipmentDataService;

        public FullScreenExercise(
            IDataService<Exercise> exerciseDataService,
            IDataService<Muscle> muscleDataService,
            IDataService<ExerciseCategory> categoryDataService,
            IDataService<ExerciseNote> notesDataService,
            IDataService<Equipment> equipmentDataService
            )
        {
            InitializeComponent();

            BindingContext = this;
            this.exerciseDataService = exerciseDataService;
            this.muscleDataService = muscleDataService;
            this.categoryDataService = categoryDataService;
            this.notesDataService = notesDataService;
            this.equipmentDataService = equipmentDataService;
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



        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Exercise = query[nameof(Models.Exercise)] as Exercise;
            MainImage = Exercise.Images.FirstOrDefault(i => i.IsMain);
            OnOpen();
        }

        private async void OnOpen()
        {
            Exercise = await exerciseDataService.Get(Exercise.Id);
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