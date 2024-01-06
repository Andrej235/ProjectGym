using AppProjectGym.Information;
using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Delete;
using AppProjectGym.Services.Mapping;
using AppProjectGym.Services.Read;
using AppProjectGym.Services.Update;
using AppProjectGym.Utilities;
using System.Linq;

namespace AppProjectGym.Pages
{
    public partial class WorkoutEditPage : ContentPage, IQueryAttributable
    {
        private delegate void RepRangeInputHandler(int bottom, int top);
        private RepRangeInputHandler repRangeInputHandler;

        private delegate void ExerciseSelectionHandler(ExerciseDisplay selectedExercise);
        private ExerciseSelectionHandler exerciseSelectionHandler;

        private delegate void NumberInputHandler(int input);
        private NumberInputHandler numberInputHandler;

        private readonly ICreateService createService;
        private readonly IReadService readService;
        private readonly IUpdateService updateService;
        private readonly IDeleteService deleteService;
        private readonly IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper;
        private readonly IEntityDisplayMapper<WorkoutSet, WorkoutSetDisplay> workoutSetDisplayMapper;

        public Workout Workout
        {
            get => workout;
            set
            {
                workout = value;
                isPublic = value.IsPublic;
                OnPropertyChanged();
            }
        }
        private Workout workout;
        private bool isPublic;

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
        private List<WorkoutSetDisplay> originalWorkoutSetDisplays;
        private bool isLoadingData;

        public WorkoutEditPage(ICreateService createService, IReadService readService, IUpdateService updateService, IDeleteService deleteService, IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper, IEntityDisplayMapper<WorkoutSet, WorkoutSetDisplay> workoutSetDisplayMapper)
        {
            InitializeComponent();
            BindingContext = this;
            this.createService = createService;
            this.readService = readService;
            this.updateService = updateService;
            this.deleteService = deleteService;
            this.exerciseDisplayMapper = exerciseDisplayMapper;
            this.workoutSetDisplayMapper = workoutSetDisplayMapper;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            isLoadingData = true;
            if (query.TryGetValue("selectedExercise", out object value) && value is Exercise exercise)
            {
                exerciseSelectionHandler(await exerciseDisplayMapper.Map(exercise));
                isLoadingData = false;
                return;
            }

            if (!query.TryGetValue("workout", out object workoutObj) || workoutObj is not Workout workout)
            {
                await NavigationService.GoToAsync("..");
                return;
            }

            Workout = workout;
            WorkoutSetDisplays = [];

            var workoutSets = await readService.Get<List<WorkoutSet>>("none", "workoutset", $"workout={workout.Id}");
            WorkoutSetDisplays.AddRange(await Task.WhenAll(workoutSets.Select(async workoutSet => await workoutSetDisplayMapper.Map(workoutSet))));

            originalWorkoutSetDisplays = WorkoutSetDisplays.DeepCopy();
            RefreshSetCollection();
            isLoadingData = false;
        }

        private void RefreshSetCollection()
        {
            setCollection.ItemsSource = null;
            setCollection.ItemsSource = WorkoutSetDisplays;
        }

        #region Dialogs
        private void OnWhiteOverlayClicked(object sender, EventArgs e)
        {
            CloseRepRangeInputDialog();
            CloseNumberInput();
        }

        #region Rep range input
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

            var setDisplay = button.BindingContext as SetDisplay;
            repRangeInputHandler = (bottom, top) =>
            {
                if (top < bottom)
                    throw new Exception("User entered an invalid rep range, top is lower than bottom");

                setDisplay.Set.RepRange_Bottom = bottom;
                setDisplay.Set.RepRange_Top = top;
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
        #endregion

        #region Number input
        private void OpenNumberInput()
        {
            singleNumberInputDialogWrapper.IsVisible = true;
            whiteOverlay.IsVisible = true;
        }

        private void CloseNumberInput()
        {
            singleNumberInputDialogWrapper.IsVisible = false;
            whiteOverlay.IsVisible = false;
        }

        private void OnNumberSubmit(object sender, EventArgs e) => HandleNumberInput(singleNumberInput.Text);

        private void OnNumberInput(object sender, EventArgs e)
        {
            if (sender is not Button button)
                return;

            WorkoutSetDisplay workoutSetDisplay = button.BindingContext as WorkoutSetDisplay;
            numberInputHandler = input =>
            {
                workoutSetDisplay.TargetSets = input;
                RefreshSetCollection();
            };

            OpenNumberInput();
        }

        private void HandleNumberInput(string inputNumberAsText)
        {
            try
            {
                if (!int.TryParse(inputNumberAsText, out int input))
                    throw new Exception("User entered an invalid number, couldn't parse to int");

                CloseNumberInput();
                numberInputHandler(input);

                bottomRepRangeInput.Text = "";
                topRepRangeInput.Text = "";
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
            }
        }
        #endregion

        #endregion

        #region Set editing
        private void OnWorkoutSetCreate(object sender, EventArgs e)
        {
            if (isLoadingData)
                return;

            WorkoutSetDisplay newWorkoutSetDisplay = new()
            {
                Set = new()
                {
                    Set = new(),
                    Exercise = new()
                },
                TargetSets = 0,
            };

            WorkoutSetDisplays.Add(newWorkoutSetDisplay);
            RefreshSetCollection();
        }

        private void OnWorkoutSetDelete(object sender, EventArgs e)
        {
            if (isLoadingData)
                return;

            if (sender is not ImageButton imageButton || imageButton.BindingContext is not WorkoutSetDisplay workoutSetDisplay)
                return;

            if (workoutSetDisplay.Id != default && workoutSetDisplay.Set.Set.Id != default)
                deleteService.Delete(workoutSetDisplay.Set.Set.Id, "set");

            WorkoutSetDisplays.Remove(workoutSetDisplay);
        }

        private async void OnSetExerciseEdit(object sender, EventArgs e)
        {
            if (isLoadingData || sender is not Button button || button.BindingContext is not SetDisplay setDisplay)
                return;

            exerciseSelectionHandler = exercise =>
            {
                setDisplay.Exercise = exercise;
                setDisplay.Set.ExerciseId = exercise.Exercise.Id;
                RefreshSetCollection();
            };

            await NavigationService.SearchAsync(true);
        } 
        #endregion

        #region Saving
        private async void OnSaveChanges(object sender, EventArgs e)
        {
            if (isLoadingData)
                return;

            List<object> updatedEntity = [];
            IEnumerable<WorkoutSetDisplay> addedWorkoutSetDisplays = WorkoutSetDisplays.Where(x => x.Id == default);

            foreach (var workoutSet in WorkoutSetDisplays.Where(x => x.Id != default))
            {
                var originalWorkoutSet = originalWorkoutSetDisplays.First(x => x.Id == workoutSet.Id);

                if (!ShallowEqual(workoutSet, originalWorkoutSet))
                {
                    updatedEntity.Add(new WorkoutSet()
                    {
                        Id = workoutSet.Id,
                        TargetSets = workoutSet.TargetSets,
                        SetId = workoutSet.Set.Set.Id,
                        WorkoutId = Workout.Id,
                    });
                }

                if (!ShallowEqual(workoutSet.Set.Set, originalWorkoutSet.Set.Set))
                    updatedEntity.Add(workoutSet.Set.Set);
            }

            foreach (var entity in updatedEntity)
                await updateService.Update(entity, entity.GetType().Name);

            foreach (var workoutSetDisplay in addedWorkoutSetDisplays)
            {
                var set = workoutSetDisplay.Set.Set;
                set.CreatorId = ClientInfo.User.Id;
                var setIdString = await createService.Add(set);

                if (!Guid.TryParse(setIdString, out Guid setId) && setId != default)
                    return;

                var workoutSet = new WorkoutSet()
                {
                    SetId = setId,
                    TargetSets = workoutSetDisplay.TargetSets,
                    WorkoutId = Workout.Id
                };
                await createService.Add(workoutSet);
            }

            WorkoutSetDisplays = [];

            if (Workout.IsPublic != isPublic)
                await updateService.Update(Workout);

            await NavigationService.GoToAsync("..");
        }

#nullable enable
        private static bool ShallowEqual<T>(T? x, T? y) where T : class?
        {
            if (x == null && y == null)
                return true;

            if ((x == null && y != null) || (x != null && y == null))
                return false;

            var properties = (x?.GetType().GetProperties().Where(x => x.Name != "Id")) ?? throw new NullReferenceException();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsValueType)
                {
                    var valueX = property.GetValue(x);
                    var valueY = property.GetValue(y);
                    if (Convert.ToString(valueX) != Convert.ToString(valueY))
                        return false;
                }
            }
            return true;
        } 
        #endregion
    }
}