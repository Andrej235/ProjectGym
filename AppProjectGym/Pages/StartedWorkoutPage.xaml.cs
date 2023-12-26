using AppProjectGym.Information;
using AppProjectGym.Models;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Mapping;
using AppProjectGym.Services.Read;
using AppProjectGym.Utilities;

namespace AppProjectGym.Pages
{
    public partial class StartedWorkoutPage : ContentPage, IQueryAttributable
    {
        private delegate void EditWeightHandler(float weightDifference);
        private EditWeightHandler editWeightHandler;

        private delegate Task CreateNewWeightHandler(float newWeight);
        private CreateNewWeightHandler createNewWeightHandler;

        //private delegate void FinishedSetHandler(FinishedSetDisplay finishedSet);
        //private FinishedSetHandler finishedSetHandler;

        private readonly IReadService readService;
        private readonly ICreateService createService;
        private readonly IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper;
        private readonly IEntityDisplayMapper<WorkoutSet, WorkoutSetDisplay> workoutSetDisplayMapper;

        public List<StartedWorkout_SetDisplay> WorkoutSetDisplays
        {
            get => workoutSetDisplays;
            set
            {
                workoutSetDisplays = value;
                OnPropertyChanged();
            }
        }
        private List<StartedWorkout_SetDisplay> workoutSetDisplays;

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

        public StartedWorkout_SetDisplay SelectedWorkoutSet
        {
            get => selectedWorkoutSet;
            set
            {
                selectedWorkoutSet = value;
                OnPropertyChanged();
            }
        }
        private StartedWorkout_SetDisplay selectedWorkoutSet;



        public StartedWorkoutPage(IReadService readService, IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper, IEntityDisplayMapper<WorkoutSet, WorkoutSetDisplay> workoutSetDisplayMapper, ICreateService createService)
        {
            InitializeComponent();
            this.readService = readService;
            this.createService = createService;
            BindingContext = this;
            this.exerciseDisplayMapper = exerciseDisplayMapper;
            this.workoutSetDisplayMapper = workoutSetDisplayMapper;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!query.TryGetValue("workout", out object workoutObj) || workoutObj is not Workout workout)
                return;

            Workout = workout;
            WorkoutSetDisplays = [];

            var workoutSets = await readService.Get<List<WorkoutSet>>("none", "workoutset", $"workout={workout.Id}");
            WorkoutSetDisplays.AddRange(await Task.WhenAll(workoutSets.Select(async workoutSet => await MapToStartedWorkoutSets(workoutSet))));

            RefreshSetCollection();
            SelectedWorkoutSet = null;
        }

        private async Task<StartedWorkout_SetDisplay> MapToStartedWorkoutSets(WorkoutSet workoutSet)
        {
            var workoutSetDisplay = await workoutSetDisplayMapper.Map(workoutSet);
            var finishedSets = new List<FinishedSetDisplay>();
            for (int i = 0; i < workoutSet.TargetSets; i++)
            {
                finishedSets.Add(new()
                {
                    FinishedReps = 0,
                    Time = 0,
                    Weight = null
                });
            }

            return new StartedWorkout_SetDisplay()
            {
                WorkoutSet = workoutSetDisplay,
                FinishedSets = finishedSets
            };
        }

        private void RefreshSetCollection()
        {
            setCollection.ItemsSource = null;
            setCollection.ItemsSource = WorkoutSetDisplays;
        }

        private void OnSetSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection[0] is not StartedWorkout_SetDisplay workoutSetDisplay)
                return;

            SelectedWorkoutSet = workoutSetDisplay;
        }

        private void OnWeightClicked(object sender, EventArgs e)
        {
            if (sender is not Button button)
                return;

            PersonalExerciseWeight selectedWeight = null;
            selectedWeight = button.BindingContext as PersonalExerciseWeight;
            if (selectedWeight is null)
            {
                if (button.BindingContext is not SetDisplay setDisplay)
                    return;

                selectedWeight = setDisplay.Weight;
            }

            editWeightHandler = weightDif =>
            {
                var newWeight = selectedWeight.Weight + weightDif;
                if (newWeight < 0)
                    return;

                weightEditorEntry.Text = newWeight.ToString();
                selectedWeight.Weight = newWeight;
            };

            createNewWeightHandler = async weight =>
            {
                PersonalExerciseWeight newWeight = new()
                {
                    Weight = weight,
                    ExerciseId = selectedWeight.ExerciseId,
                    UserId = ClientInfo.User.Id,
                };

                if (await createService.Add(newWeight, "weight") == default)
                    return;

                var setsUsingWeight = WorkoutSetDisplays.Where(x => x.WorkoutSet.Set.Weight.ExerciseId == newWeight.ExerciseId).Select(x => x.WorkoutSet.Set);
                var supersetsUsingWeight = WorkoutSetDisplays.Where(x => x.WorkoutSet.Superset?.Weight.ExerciseId == newWeight.ExerciseId).Select(x => x.WorkoutSet.Superset);

                foreach (var set in setsUsingWeight)
                    set.Weight = newWeight;

                foreach (var superset in supersetsUsingWeight)
                    superset.Weight = newWeight;

                if (SelectedWorkoutSet.WorkoutSet.Set.Weight.ExerciseId == newWeight.ExerciseId)
                {
                    SelectedWorkoutSet.WorkoutSet.Set.Weight = newWeight;
                    SelectedWorkoutSet = selectedWorkoutSet.DeepCopy();
                }

                if (SelectedWorkoutSet.WorkoutSet.Superset.Weight.ExerciseId == newWeight.ExerciseId)
                {
                    SelectedWorkoutSet.WorkoutSet.Superset.Weight = newWeight;
                    SelectedWorkoutSet = selectedWorkoutSet.DeepCopy();
                }

                CloseWeighEditorDialog();
                RefreshSetCollection();
            };

            OpenWeighEditorDialog(selectedWeight.Weight);
        }

        private void OnWeightEdited(object sender, EventArgs e)
        {
            if (sender is not Button button || !float.TryParse(button.Text, out float weightDif))
                return;

            editWeightHandler(weightDif);
        }

        private void OpenWeighEditorDialog(float currentWeight = 0)
        {
            weightEditorDialogWrapper.IsVisible = true;
            weightEditorEntry.Text = currentWeight.ToString();
            whiteOverlay.IsVisible = true;
        }

        private void CloseWeighEditorDialog()
        {
            weightEditorDialogWrapper.IsVisible = false;
            whiteOverlay.IsVisible = false;
        }

        private void OnWhiteOverlayClicked(object sender, EventArgs e) => CloseWeighEditorDialog();

        private async void OnWeightCreate(object sender, EventArgs e)
        {
            if (!float.TryParse(weightEditorEntry.Text, out float newWeight))
                return;

            try
            {
                await createNewWeightHandler(newWeight);
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
            }
        }

        private bool isDoingASet = false;
        private void OnTimerToggled(object sender, EventArgs e)
        {
            isDoingASet = !isDoingASet;

            if (isDoingASet)
                StartTimer(90);
        }

        private async void StartTimer(float distanceFromEdge)
        {
            timerLabel.Text = "0";
            Vector2 vector = new(0, -1);
            vector.Normalize();

            await Task.Run(() => innerCircle.ScaleTo(1, 300, Easing.SpringIn));
            await innerCircle.TranslateTo(0, vector.Y * distanceFromEdge, 400, Easing.CubicIn);

            var currentTick = 5;
            var currentTime = 0;
            do
            {
                await innerCircle.TranslateTo(vector.X * distanceFromEdge, vector.Y * distanceFromEdge, 5);
                vector.Rotate(6);

                currentTick++;
                if (currentTick >= 60)
                {
                    currentTime++;
                    currentTick = 0;
                    UpdateTimer(currentTime);
                }
            } while (isDoingASet);

            EndTimer();
        }

        private async void UpdateTimer(float seconds)
        {
            await timerLabel.ScaleTo(1.25, 125, Easing.CubicIn);
            timerLabel.Text = seconds.ToString("#");
            await timerLabel.ScaleTo(1, 250, Easing.CubicOut);
        }

        private void EndTimer()
        {
            innerCircle.TranslateTo(0, 0, 100);
            innerCircle.ScaleTo(2.75, 500, Easing.SpringOut);

            if (activeWorkoutSet is null)
                return;

            var finishedSet = activeWorkoutSet.FinishedSets.FirstOrDefault(x => x.Time == 0);
            if (finishedSet is null || !int.TryParse(timerLabel.Text, out int time))
                return;

            finishedSet.Time = time;
            finishedSet.Weight = activeWorkoutSet.WorkoutSet.Set.Weight.DeepCopy();

            finishedSetDialogWrapper.BindingContext = null;
            finishedSetDialogWrapper.BindingContext = finishedSet;
            OpenFinishedSetDialog();
        }

        public class Vector2(float x, float y)
        {
            public float X { get; private set; } = x;
            public float Y { get; private set; } = y;

            public void Normalize()
            {
                float length = (float)Math.Sqrt(X * X + Y * Y);

                if (length != 0)
                {
                    X /= length;
                    Y /= length;
                }
            }

            public void Rotate(float degrees)
            {
                float radians = (float)(degrees * Math.PI / 180.0);
                float newX = X * (float)Math.Cos(radians) - Y * (float)Math.Sin(radians);
                float newY = X * (float)Math.Sin(radians) + Y * (float)Math.Cos(radians);

                X = newX;
                Y = newY;
            }

            public static float GetRotationAngle(Vector2 vector1, Vector2 vector2)
            {
                float crossProduct = vector1.X * vector2.Y - vector1.Y * vector2.X;
                float dotProduct = vector1.X * vector2.X + vector1.Y * vector2.Y;

                float angleRadians = (float)Math.Atan2(crossProduct, dotProduct);
                float angleDegrees = (float)(angleRadians * 180.0 / Math.PI);

                return angleDegrees;
            }
            public float GetRotationAngle(Vector2 vector2) => GetRotationAngle(this, vector2);
            public float GetRotationAngle()
            {
                var vector2 = new Vector2(0, -1);
                vector2.Normalize();

                return GetRotationAngle(this, vector2);
            }

            public float Magnitude => (float)Math.Sqrt(X * X + Y * Y);

            public override string ToString() => $"({X}, {Y})";
        }

        StartedWorkout_SetDisplay activeWorkoutSet = null;
        FinishedSetDisplay activeFinishedSet = null;
        private void OnSetStarted(object sender, EventArgs e)
        {
            if (sender is not Button button || button.BindingContext is not StartedWorkout_SetDisplay startedWorkout_SetDisplay)
                return;

            activeWorkoutSet = startedWorkout_SetDisplay;
            activeFinishedSet = startedWorkout_SetDisplay.FinishedSets.FirstOrDefault(x => x.Time == 0);
            if (activeFinishedSet is null)
                return;

            OpenTimer();
        }

        private void OpenTimer() => timerWrapper.IsVisible = true;

        private void CloseTimer() => timerWrapper.IsVisible = false;

        private void OpenFinishedSetDialog()
        {
            completedRepsEntry.Text = "0";
            finishedSetDialogWrapper.IsVisible = true;
        }

        private void CloseFinishedSetDialog() => finishedSetDialogWrapper.IsVisible = false;

        private void OnExitFinishedSetDialog(object sender, EventArgs e)
        {
            if (!int.TryParse(timerLabel.Text, out int time) || time <= 0 || !int.TryParse(completedRepsEntry.Text, out int completedReps) || completedReps <= 0)
                return;

            activeFinishedSet.FinishedReps = completedReps;

            CloseFinishedSetDialog();
            CloseTimer();
            RefreshSetCollection();

            SelectedWorkoutSet = null;
            SelectedWorkoutSet = activeWorkoutSet;

            activeWorkoutSet = null;
            activeFinishedSet = null;
        }

        private void OnStartNextSetPressed(object sender, EventArgs e)
        {

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            CloseTimer();
            innerCircle.TranslateTo(0, 0, 100);
            innerCircle.ScaleTo(2.75, 500, Easing.SpringOut);
            activeWorkoutSet = null;
            activeFinishedSet = null;
        }
    }
}

namespace AppProjectGym.Models
{
    public class StartedWorkout_SetDisplay
    {
        public WorkoutSetDisplay WorkoutSet { get; set; }
        public List<FinishedSetDisplay> FinishedSets { get; set; } //At the start make this have the correct number of sets (== WorkoutSet.TargetSets)
    }

    public class FinishedSetDisplay
    {
        public int FinishedReps { get; set; }
        public int Time { get; set; } //If this prop is 0 display not yet attempted
        public PersonalExerciseWeight Weight { get; set; }
    }
}