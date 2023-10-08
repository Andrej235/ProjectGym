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

        public FullScreenExercise(
            IDataService<Exercise> exerciseDataService,
            IDataService<Muscle> muscleDataService,
            IDataService<ExerciseCategory> categoryDataService,
            IDataService<ExerciseNote> notesDataService
            )
        {
            InitializeComponent();

            BindingContext = this;
            this.exerciseDataService = exerciseDataService;
            this.muscleDataService = muscleDataService;
            this.categoryDataService = categoryDataService;
            this.notesDataService = notesDataService;
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
            Debug.WriteLine("---> ApplyQueryAttributes");
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

        private async void LoadNotes()
        {
            var a = await Task.WhenAll(Exercise.NoteIds.Select(notesDataService.Get));

            Notes = a.ToList();
        }
    }
}