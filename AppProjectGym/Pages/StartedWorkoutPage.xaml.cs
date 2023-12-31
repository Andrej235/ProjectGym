using AppProjectGym.Information;
using AppProjectGym.LocalDatabase;
using AppProjectGym.LocalDatabase.Models;
using AppProjectGym.Models;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Mapping;
using AppProjectGym.Services.Read;
using AppProjectGym.Utilities;
using AppProjectGym.Utilities.Models;

namespace AppProjectGym.Pages
{
    public partial class StartedWorkoutPage : ContentPage, IQueryAttributable
    {
        private readonly FinishedWorkoutContext context;
        private readonly IReadService readService;
        private readonly ICreateService createService;
        private readonly IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper;
        private readonly IEntityDisplayMapper<WorkoutSet, StartedWorkout_SetDisplay> startedWorkoutSetDisplayMapper;

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
        private FinishedWorkout finishedWorkout;

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

        public StartedWorkoutPage(IReadService readService, IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper, IEntityDisplayMapper<WorkoutSet, StartedWorkout_SetDisplay> startedWorkoutSetDisplayMapper, ICreateService createService)
        {
            InitializeComponent();
            BindingContext = this;

            context = FinishedWorkoutContext.Context;
            this.readService = readService;
            this.createService = createService;
            this.exerciseDisplayMapper = exerciseDisplayMapper;
            this.startedWorkoutSetDisplayMapper = startedWorkoutSetDisplayMapper;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!query.TryGetValue("workout", out object workoutObj) || workoutObj is not Workout workout)
                return;

            Workout = workout;
            WorkoutSetDisplays = [];

            var workoutSets = await readService.Get<List<WorkoutSet>>("none", "workoutset", $"workout={workout.Id}");
            WorkoutSetDisplays.AddRange(await Task.WhenAll(workoutSets.Select(async workoutSet => await startedWorkoutSetDisplayMapper.Map(workoutSet))));

            RefreshSetCollection();
            SelectedWorkoutSet = null;

            finishedWorkout = new()
            {
                Name = Workout.Name,
                WorkoutId = Workout.Id,
                WorkoutSets = []
            };
            context.FinishedWorkouts.Add(finishedWorkout);

            var finishedSetsJSON = context.FinishedSets.GetJSONForm();
            var finishedWorkoutSetsJSON = context.FinishedWorkoutSets.GetJSONForm();
            var finishedWorkoutsJSON = context.FinishedWorkouts.GetJSONForm();
        }

        private void OnWorkoutSetDisplaySelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection[0] is not StartedWorkout_SetDisplay workoutSetDisplay)
                return;

            SelectedWorkoutSet = workoutSetDisplay;
        }

        #region Weight
        private delegate void EditWeightHandler(float weightDifference);
        private EditWeightHandler editWeightHandler;

        private delegate Task CreateNewWeightHandler(float newWeight);
        private CreateNewWeightHandler createNewWeightHandler;

        private void OpenWeighEditorDialog(float currentWeight = 0)
        {
            weightEditorDialogWrapper.IsVisible = true;
            whiteOverlay.IsVisible = true;
            weightEditorEntry.Text = currentWeight.ToString();
        }

        private void CloseWeighEditorDialog()
        {
            weightEditorDialogWrapper.IsVisible = false;
            whiteOverlay.IsVisible = false;
        }

        private void OnWhiteOverlayClicked(object sender, EventArgs e)
        {
            if (weightEditorDialogWrapper.IsVisible)
                CloseWeighEditorDialog();

            if (finishWorkoutConfirmDialog.IsVisible)
                CloseFinishWorkoutConfirmDialog();
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

                if (activeWorkoutSet is not null)
                {
                    activeFinishedSet.Weight = newWeight;
                    finishedSetDialogWrapper.BindingContext = null;
                    finishedSetDialogWrapper.BindingContext = activeFinishedSet;
                }
            };

            OpenWeighEditorDialog(selectedWeight.Weight);
        }

        private void OnWeightEditedUsingButtons(object sender, EventArgs e)
        {
            if (sender is not Button button || !float.TryParse(button.Text, out float weightDif))
                return;

            editWeightHandler(weightDif);
        }

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
        #endregion

        #region Started set
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

            OpenTimer(startedWorkout_SetDisplay.WorkoutSet.Set);
        }

        private void OnCancelStartedSet(object sender, EventArgs e)
        {
            CloseTimer();
            innerCircle.TranslateTo(0, 0, 100);
            innerCircle.ScaleTo(2.75, 500, Easing.SpringOut);
            activeWorkoutSet = null;
            activeFinishedSet = null;
        }

        #region Timer
        private bool isDoingASet = false;

        private void OpenTimer(SetDisplay setDisplay)
        {
            timerWrapper.BindingContext = setDisplay;
            timerWrapper.IsVisible = true;
        }

        private void CloseTimer()
        {
            timerWrapper.IsVisible = false;
            timerWrapper.BindingContext = null;
        }

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
        #endregion

        #region Finished set
        private void OpenFinishedSetDialog()
        {
            completedRepsEntry.Text = "";
            finishedSetDialogWrapper.IsVisible = true;
        }

        private void CloseFinishedSetDialog() => finishedSetDialogWrapper.IsVisible = false;

        private void OnBackButtonPressed_FinishedSetDialog(object sender, EventArgs e)
        {
            if (!int.TryParse(timerLabel.Text, out int time) || time <= 0)
                return;

            if (!int.TryParse(completedRepsEntry.Text, out int completedReps) || completedReps <= 0)
            {
                OpenRepsForcedInputDialog();
                return;
            }

            SaveActiveFinishedSetChanges(completedReps);
        }

        private void OnStartNextSetButtonPressed(object sender, EventArgs e)
        {

        }

        private void SaveActiveFinishedSetChanges(int completedReps)
        {
            activeFinishedSet.FinishedReps = completedReps;

            CloseFinishedSetDialog();
            CloseTimer();
            RefreshSetCollection();

            var activeFinishedWorkoutSet = finishedWorkout.WorkoutSets.FirstOrDefault(x => x.WorkoutSetId == activeWorkoutSet.WorkoutSet.Id);
            if (activeFinishedWorkoutSet == null)
            {
                activeFinishedWorkoutSet = new()
                {
                    WorkoutSetId = activeWorkoutSet.WorkoutSet.Id,
                    Workout = finishedWorkout,
                    Sets = [],
                };
                context.FinishedWorkoutSets.Add(activeFinishedWorkoutSet);
            }

            FinishedSet finishedSet = new()
            {
                SetId = activeWorkoutSet.WorkoutSet.Set.Set.Id,
                Reps = activeFinishedSet.FinishedReps,
                Time = activeFinishedSet.Time,
                Weight = activeFinishedSet.Weight.Weight,
                WorkoutSet = activeFinishedWorkoutSet
            };
            context.FinishedSets.Add(finishedSet);
            context.SaveChanges();

            SelectedWorkoutSet = null;
            SelectedWorkoutSet = activeWorkoutSet;

            activeWorkoutSet = null;
            activeFinishedSet = null;
        }

        #region Forced reps dialog
        private void OpenRepsForcedInputDialog()
        {
            completedRepsForcedInputDialogWrapper.IsVisible = true;
            whiteOverlay.IsVisible = true;
        }

        private void CloseRepsForcedInputDialog()
        {
            completedRepsForcedInputDialogWrapper.IsVisible = false;
            whiteOverlay.IsVisible = false;
        }

        private void OnSubmitCompletedRepsForcedDialog(object sender, EventArgs e)
        {
            if (!int.TryParse(completedRepsForcedInputDialogEntry.Text, out int reps))
                return;

            SaveActiveFinishedSetChanges(reps);
            CloseRepsForcedInputDialog();
        }
        #endregion

        #endregion

        #endregion

        #region Finish Workout
        private void OpenFinishWorkoutConfirmDialog()
        {
            finishWorkoutConfirmDialog.IsVisible = true;
            whiteOverlay.IsVisible = true;
        }

        private void CloseFinishWorkoutConfirmDialog()
        {
            finishWorkoutConfirmDialog.IsVisible = false;
            whiteOverlay.IsVisible = false;
        }

        private void OnFinishWorkoutClicked(object sender, EventArgs e) => OpenFinishWorkoutConfirmDialog();

        private void OnCancelConfirmDialog(object sender, EventArgs e) => CloseFinishWorkoutConfirmDialog();

        private void OnFinishWorkoutConfirmed(object sender, EventArgs e)
        {
            finishedWorkout.DateTime = DateTime.Now;
            context.SaveChanges();
            CloseFinishWorkoutConfirmDialog();
        }
        #endregion

        private void RefreshSetCollection()
        {
            setCollection.ItemsSource = null;
            setCollection.ItemsSource = WorkoutSetDisplays;
        }
    }
}