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
        private List<WorkoutSetDisplay> originalWorkoutSetDisplays;

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
            if (query.TryGetValue("selectedExercise", out object value) && value is Exercise exercise)
            {
                exerciseSelectionHandler(await exerciseDisplayMapper.Map(exercise));
                return;
            }

            if (!query.TryGetValue("workout", out object workoutObj) || workoutObj is not Workout workout)
                return;

            Workout = workout;
            WorkoutSetDisplays = [];

            var workoutSets = await readService.Get<List<WorkoutSet>>("none", "workoutset", $"workout={workout.Id}");
            WorkoutSetDisplays.AddRange(await Task.WhenAll(workoutSets.Select(async workoutSet => await workoutSetDisplayMapper.Map(workoutSet))));

            originalWorkoutSetDisplays = WorkoutSetDisplays.DeepCopy();
            RefreshSetCollection();
        }

        private void OnWhiteOverlayClicked(object sender, EventArgs e)
        {
            CloseRepRangeInputDialog();
            CloseNumberInput();
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

        private void OnWorkoutSetCreate(object sender, EventArgs e)
        {
            WorkoutSetDisplay newWorkoutSetDisplay = new()
            {
                Set = new()
                {
                    Set = new(),
                    Exercise = new()
                },
                Superset = null,
                TargetSets = 0
            };

            WorkoutSetDisplays.Add(newWorkoutSetDisplay);
            RefreshSetCollection();
        }

        private void OnWorkoutSetDelete(object sender, EventArgs e)
        {
            if (sender is not ImageButton imageButton || imageButton.BindingContext is not WorkoutSetDisplay workoutSetDisplay)
                return;

            if (workoutSetDisplay.Id != default)
            {
                if (workoutSetDisplay.Set.Set.Id != default)
                    deleteService.Delete(workoutSetDisplay.Set.Set.Id, "set");

                if (workoutSetDisplay.Superset != null && workoutSetDisplay.Superset.Set.Id != default)
                    deleteService.Delete(workoutSetDisplay.Superset.Set.Id, "set");
            }

            WorkoutSetDisplays.Remove(workoutSetDisplay);
        }

        private void RefreshSetCollection()
        {
            setCollection.ItemsSource = null;
            setCollection.ItemsSource = WorkoutSetDisplays;
            //WorkoutSetDisplays = workoutSetDisplays;
        }

        private async void OnSetExerciseEdit(object sender, EventArgs e)
        {
            if (sender is not Button button || button.BindingContext is not SetDisplay setDisplay)
                return;

            exerciseSelectionHandler = exercise =>
            {
                setDisplay.Exercise = exercise;
                setDisplay.Set.ExerciseId = exercise.Exercise.Id;
                RefreshSetCollection();
            };

            await NavigationService.SearchAsync(true);
        }

        private async void OnWorkoutSetSave(object sender, EventArgs e)
        {
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
                        SuperSetId = workoutSet.Superset?.Set.Id,
                        WorkoutId = Workout.Id
                    });
                }

                if (workoutSet.Superset != null && originalWorkoutSet.Superset == null)
                {
                    workoutSet.Superset.Set.CreatorId = ClientInfo.User.Id;
                    var supersetIdString = await createService.Add(workoutSet.Superset.Set);

                    if (Guid.TryParse(supersetIdString.Replace("\"", ""), out Guid supersetId))
                        updatedEntity.Add(new WorkoutSet()
                        {
                            Id = workoutSet.Id,
                            TargetSets = workoutSet.TargetSets,
                            SetId = workoutSet.Set.Set.Id,
                            SuperSetId = supersetId,
                            WorkoutId = Workout.Id
                        });
                }

                if (workoutSet.Superset == null && originalWorkoutSet.Superset != null)
                {
                    updatedEntity.Add(new WorkoutSet()
                    {
                        Id = workoutSet.Id,
                        TargetSets = workoutSet.TargetSets,
                        SetId = workoutSet.Set.Set.Id,
                        SuperSetId = null,
                        WorkoutId = Workout.Id
                    });
                    await deleteService.Delete(originalWorkoutSet.Superset.Set);
                }

                if (!ShallowEqual(workoutSet.Set.Set, originalWorkoutSet.Set.Set))
                    updatedEntity.Add(workoutSet.Set.Set);

                if (workoutSet.Superset != null && originalWorkoutSet.Superset != null && !ShallowEqual(workoutSet.Superset?.Set, originalWorkoutSet.Superset?.Set))
                    updatedEntity.Add(workoutSet.Superset.Set);
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
                    SuperSetId = workoutSetDisplay.Superset?.Set.Id,
                    TargetSets = workoutSetDisplay.TargetSets,
                    WorkoutId = Workout.Id
                };
                await createService.Add(workoutSet);
            }

            WorkoutSetDisplays = [];
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

        private void OnToggleSuperset(object sender, EventArgs e)
        {
            if (sender is not Button button || button.BindingContext is not WorkoutSetDisplay workoutSetDisplay)
                return;

            workoutSetDisplay.Superset = workoutSetDisplay.Superset is null ? new()
            {
                Set = new()
            } : null;
            RefreshSetCollection();
        }
    }
}