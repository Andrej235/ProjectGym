using AppProjectGym.Information;
using AppProjectGym.LocalDatabase;
using AppProjectGym.LocalDatabase.Models;
using AppProjectGym.Models;
using AppProjectGym.Services;
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

        protected override void OnDisappearing()
        {
            if (finishedWorkout != null && finishedWorkout.DateTime == default)
                context?.Delete(finishedWorkout);

            base.OnDisappearing();
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

                foreach (var set in setsUsingWeight)
                    set.Weight = newWeight;

                if (SelectedWorkoutSet.WorkoutSet.Set.Weight.ExerciseId == newWeight.ExerciseId)
                {
                    SelectedWorkoutSet.WorkoutSet.Set.Weight = newWeight;
                    SelectedWorkoutSet = selectedWorkoutSet.DeepCopy();
                }

                CloseWeighEditorDialog();
                RefreshSetCollection();

                if (activeWorkoutSetDisplay is not null)
                {
                    activeFinishedSetDisplay.Weight = newWeight;
                    finishedSetDialogWrapper.BindingContext = null;
                    finishedSetDialogWrapper.BindingContext = activeFinishedSetDisplay;
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
        StartedWorkout_SetDisplay activeWorkoutSetDisplay = null;
        FinishedSetDisplay activeFinishedSetDisplay = null;
        FinishedSet activeFinishedSet = null;

        private void OnSetStarted(object sender, EventArgs e)
        {
            if (sender is not Button button || button.BindingContext is not StartedWorkout_SetDisplay startedWorkout_SetDisplay)
                return;

            activeWorkoutSetDisplay = startedWorkout_SetDisplay;
            activeFinishedSetDisplay = startedWorkout_SetDisplay.FinishedSets.FirstOrDefault(x => x.Time == 0);
            if (activeFinishedSetDisplay is null)
                return;

            OpenTimer(startedWorkout_SetDisplay.WorkoutSet.Set);
        }

        private void OnCancelStartedSet(object sender, EventArgs e)
        {
            CloseTimer();
            innerCircle.TranslateTo(0, 0, 100);
            innerCircle.ScaleTo(2.75, 500, Easing.SpringOut);
            activeWorkoutSetDisplay = null;
            activeFinishedSetDisplay = null;
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
            isResting = false;
            timerLabel.Text = "00";
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
            timerLabel.Text = FormatTime((int)seconds);// seconds.ToString("#");
            await timerLabel.ScaleTo(1, 250, Easing.CubicOut);
        }

        static string FormatTime(int seconds) => seconds switch
        {
            < 60 => $"{seconds:D2}",
            < 3600 => $"{seconds / 60:D2}:{seconds % 60:D2}",
            _ => $"{seconds / 3600:D2}:{seconds % 3600 / 60:D2}:{seconds % 60:D2}",
        };

        static bool TryParseFormattedTime(string formattedTime, out int result)
        {
            try
            {
                string[] timeComponents = formattedTime.Split(':');
                result = timeComponents.Length switch
                {
                    1 => int.Parse(timeComponents[0]),// Seconds only
                    2 => int.Parse(timeComponents[0]) * 60 + int.Parse(timeComponents[1]),// MM:SS
                    3 => int.Parse(timeComponents[0]) * 3600 + int.Parse(timeComponents[1]) * 60 + int.Parse(timeComponents[2]),// HH:MM:SS
                    _ => throw new ArgumentException("Invalid time format"),
                };
                return true;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                result = -1;
                return false;
            }
        }

        private void EndTimer()
        {
            innerCircle.TranslateTo(0, 0, 100);
            innerCircle.ScaleTo(2.75, 500, Easing.SpringOut);

            if (activeWorkoutSetDisplay is null)
                return;

            var finishedSet = activeWorkoutSetDisplay.FinishedSets.FirstOrDefault(x => x.Time == 0);
            if (finishedSet is null || !TryParseFormattedTime(timerLabel.Text, out int time))
                return;

            finishedSet.Time = time;
            finishedSet.Weight = activeWorkoutSetDisplay.WorkoutSet.Set.Weight.DeepCopy();
            StartRestPeriod(finishedSet);

            finishedSetDialogWrapper.BindingContext = null;
            finishedSetDialogWrapper.BindingContext = finishedSet;

            OpenFinishedSetDialog();
        }

        private bool isResting;
        private int restTimer;
        private async void StartRestPeriod(FinishedSetDisplay finishedSetDisplay)
        {
            isResting = true;
            restTimer = 0;
            finishedSetDisplay.RestTime = 0;
            while (isResting)
            {
                await Task.Delay(1000);
                restTimer++;
            }

            finishedSetDisplay.RestTime = restTimer;

            if (activeFinishedSet != null)
                activeFinishedSet.RestTime = restTimer;
            activeFinishedSet = null;
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
            if (!TryParseFormattedTime(timerLabel.Text, out int time) || time <= 0)
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
            activeFinishedSetDisplay.FinishedReps = completedReps;

            CloseFinishedSetDialog();
            CloseTimer();
            RefreshSetCollection();

            var activeFinishedWorkoutSet = finishedWorkout.WorkoutSets.FirstOrDefault(x => x.WorkoutSetId == activeWorkoutSetDisplay.WorkoutSet.Id);
            if (activeFinishedWorkoutSet == null)
            {
                activeFinishedWorkoutSet = new()
                {
                    WorkoutSetId = activeWorkoutSetDisplay.WorkoutSet.Id,
                    Workout = finishedWorkout,
                    Sets = [],
                };
                context.FinishedWorkoutSets.Add(activeFinishedWorkoutSet);
            }

            FinishedSet finishedSet = new()
            {
                SetId = activeWorkoutSetDisplay.WorkoutSet.Set.Set.Id,
                Reps = activeFinishedSetDisplay.FinishedReps,
                Time = activeFinishedSetDisplay.Time,
                Weight = activeFinishedSetDisplay.Weight.Weight,
                WorkoutSet = activeFinishedWorkoutSet
            };
            context.FinishedSets.Add(finishedSet);
            context.SaveChanges();

            SelectedWorkoutSet = null;
            SelectedWorkoutSet = activeWorkoutSetDisplay;

            activeWorkoutSetDisplay = null;
            activeFinishedSetDisplay = null;
            activeFinishedSet = finishedSet;
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

        private async void OnFinishWorkoutConfirmed(object sender, EventArgs e)
        {
            isResting = false;
            if (activeFinishedSet != null)
                activeFinishedSet.RestTime = restTimer;

            finishedWorkout.DateTime = DateTime.Now;
            context.SaveChanges();
            CloseFinishWorkoutConfirmDialog();
            await NavigationService.GoToAsync("../..");
        }
        #endregion

        private void RefreshSetCollection()
        {
            setCollection.ItemsSource = null;
            setCollection.ItemsSource = WorkoutSetDisplays;
        }
    }
}