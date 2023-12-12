using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Delete;
using AppProjectGym.Services.Read;
using AppProjectGym.Services.Update;
using AppProjectGym.Utilities;

namespace AppProjectGym.Pages
{
    public partial class WorkoutEditPage : ContentPage, IQueryAttributable
    {
        private delegate void RepRangeInputHandler(int bottom, int top);
        private RepRangeInputHandler repRangeInputHandler;

        private readonly ICreateService createService;
        private readonly IReadService readService;
        private readonly IUpdateService updateService;
        private readonly IDeleteService deleteService;
        private readonly ExerciseDisplayMapper exerciseDisplayMapper;

        public Workout Workout
        {
            get => workout;
            set
            {
                workout = value;
                OnPropertyChanged();
            }
        }
        private Workout workout;

        public List<WorkoutSetDisplay> WorkoutSetDisplays
        {
            get => workoutSetDisplays;
            set
            {
                workoutSetDisplays = value;
                OnPropertyChanged();
            }
        }
        private List<WorkoutSetDisplay> workoutSetDisplays;



        public WorkoutEditPage(ICreateService createService, IReadService readService, IUpdateService updateService, IDeleteService deleteService, ExerciseDisplayMapper exerciseDisplayMapper)
        {
            InitializeComponent();
            BindingContext = this;
            this.createService = createService;
            this.readService = readService;
            this.updateService = updateService;
            this.deleteService = deleteService;
            this.exerciseDisplayMapper = exerciseDisplayMapper;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!query.TryGetValue("workout", out object workoutObj) || workoutObj is not Models.Workout workout)
                return;

            WorkoutSetDisplays ??= [];
            var workoutSets = await readService.Get<List<WorkoutSet>>("set", "workoutset", $"workout={workout.Id}");

            foreach (var workoutSet in workoutSets)
            {
                var setDisplay = new SetDisplay { Set = await readService.Get<Set>("exercise", $"set/{workoutSet.SetId}") };
                setDisplay.Exercise = await exerciseDisplayMapper.Map(await readService.Get<Exercise>("image", $"exercise/{setDisplay.Set.ExerciseId}"));

                WorkoutSetDisplay workoutSetDisplay = new()
                {
                    Id = workoutSet.Id,
                    TargetSets = workoutSet.TargetSets,
                    Set = setDisplay,
                };

                if (workoutSet.SuperSetId != null)
                {
                    var supersetDisplay = new SupersetDisplay { Superset = await readService.Get<Superset>("exercise", $"superset/{workoutSet.SuperSetId}") };
                    var superSetsSet = await readService.Get<Set>("exercise", $"set/{supersetDisplay.Superset.SetId}");
                    supersetDisplay.Exercise = await exerciseDisplayMapper.Map(await readService.Get<Exercise>("image", $"exercise/{superSetsSet.ExerciseId}"));
                    workoutSetDisplay.Superset = supersetDisplay;
                }

                WorkoutSetDisplays.Add(workoutSetDisplay);
            }
            Workout = workout;

            setCollection.ItemsSource = null;
            setCollection.ItemsSource = WorkoutSetDisplays;
        }

        private void OnWhiteOverlayClicked(object sender, EventArgs e)
        {
            CloseRepRangeInputDialog();
        }

        private void OpenRepRangeInputDialog()
        {
            repRangeInputDialogWrapper.IsVisible = true;
            whiteOverlay.IsVisible = true;
        }

        private void CloseRepRangeInputDialog()
        {
            repRangeInputDialogWrapper.IsVisible = false;
            whiteOverlay.IsVisible = false;
        }

        private void OnRepRangeEdit(object sender, EventArgs e)
        {
            if (sender is not Button button)
                return;

            WorkoutSetDisplay workoutSetDisplay = button.BindingContext as WorkoutSetDisplay;
            repRangeInputHandler = (bottom, top) =>
            {
                if (top < bottom)
                    throw new Exception("User entered an invalid rep range, top is lower than bottom");

                workoutSetDisplay.Set.Set.RepRange_Bottom = bottom;
                workoutSetDisplay.Set.Set.RepRange_Top = top;
                RefreshSetCollection();
            };

            OpenRepRangeInputDialog();
        }

        private void OnRepRangeSubmit(object sender, EventArgs e) => HandleRepRangeInput(bottomRepRangeInput.Text, topRepRangeInput.Text);

        private void HandleRepRangeInput(string bottomText, string topText)
        {
            try
            {
                if (!int.TryParse(bottomText, out int bottom) || !int.TryParse(topText, out int top))
                    throw new Exception("User entered an invalid rep range, couldn't parse to int");

                CloseRepRangeInputDialog();
                repRangeInputHandler(bottom, top);

                bottomRepRangeInput.Text = "";
                topRepRangeInput.Text = "";
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
            }
        }

        private void OnWorkoutSetCreate(object sender, EventArgs e)
        {
            WorkoutSetDisplay newWorkoutSetDisplay = new()
            {
                Set = new()
                {
                    Set = new()
                },
                Superset = null,
                TargetSets = 0
            };

            WorkoutSetDisplays.Add(newWorkoutSetDisplay);
            RefreshSetCollection();
        }

        private void RefreshSetCollection()
        {
            setCollection.ItemsSource = null;
            setCollection.ItemsSource = WorkoutSetDisplays;
        }

        private async void OnSetExerciseEdit(object sender, EventArgs e)
        {
            await NavigationService.SearchAsync(true);
        }
    }
}