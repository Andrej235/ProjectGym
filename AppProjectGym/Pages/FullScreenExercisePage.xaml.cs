using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Delete;
using AppProjectGym.Services.Read;
using AppProjectGym.Utilities;

namespace AppProjectGym.Pages
{
    public partial class FullScreenExercisePage : ContentPage, IQueryAttributable
    {
        private delegate Task ConfirmHandler(bool input);
        private ConfirmHandler confirmHandler;

        private readonly IReadService readService;
        private readonly IDeleteService deleteService;
        private readonly ExerciseDisplayMapper exerciseDisplayMapper;

        public FullScreenExercisePage(IReadService readService, IDeleteService deleteService, ExerciseDisplayMapper exerciseDisplayMapper)
        {
            InitializeComponent();
            BindingContext = this;
            this.readService = readService;
            this.deleteService = deleteService;
            this.exerciseDisplayMapper = exerciseDisplayMapper;
        }

        public bool IsInSelectionMode
        {
            get => isInSelectionMode;
            private set
            {
                isInSelectionMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotInSelectionMode));
            }
        }
        public bool IsNotInSelectionMode => !IsInSelectionMode;
        private bool isInSelectionMode;

        public Exercise Exercise { get; private set; }

        public ExerciseDisplay ExerciseDisplay
        {
            get => exerciseDisplay;
            set
            {
                exerciseDisplay = value;
                OnPropertyChanged();
            }
        }
        private ExerciseDisplay exerciseDisplay;

        public List<MuscleGroupDisplay> PrimaryMuscleDisplays
        {
            get => primaryMuscleDisplays;
            set
            {
                primaryMuscleDisplays = value;
                OnPropertyChanged();
            }
        }
        private List<MuscleGroupDisplay> primaryMuscleDisplays;

        public List<MuscleGroupDisplay> SecondaryMuscleDisplays
        {
            get => secondaryMuscleDisplays;
            set
            {
                secondaryMuscleDisplays = value;
                OnPropertyChanged();
            }
        }
        private List<MuscleGroupDisplay> secondaryMuscleDisplays;

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
            IsInSelectionMode = query.TryGetValue("selectionMode", out object selectionModeObj) && selectionModeObj is bool selectionMode && selectionMode;

            Exercise = await readService.Get<Exercise>("none", $"exercise/{id}");
            ExerciseDisplay = await exerciseDisplayMapper.Map(Exercise);

            var primaryMuscleGroups = await readService.Get<List<MuscleGroup>>("none", "musclegroup", $"primary={Exercise.Id}");
            var primaryMuscles = await readService.Get<List<Muscle>>("none", "muscle", $"primary={Exercise.Id}");
            PrimaryMuscleDisplays = primaryMuscleGroups.Select(x => new MuscleGroupDisplay()
            {
                Id = x.Id,
                Name = x.Name,
                Muscles = primaryMuscles.Where(y => y.MuscleGroupId == x.Id)
            }).ToList();

            var secondaryMuscleGroups = await readService.Get<List<MuscleGroup>>("none", "musclegroup", $"secondary={Exercise.Id}");
            var secondaryMuscles = await readService.Get<List<Muscle>>("none", "muscle", $"secondary={Exercise.Id}");
            SecondaryMuscleDisplays = secondaryMuscleGroups.Select(x => new MuscleGroupDisplay()
            {
                Id = x.Id,
                Name = x.Name,
                Muscles = secondaryMuscles.Where(y => y.MuscleGroupId == x.Id)
            }).ToList();

            Notes = await readService.Get<List<ExerciseNote>>("none", "note", $"exercise={Exercise.Id}");
            Equipment = await readService.Get<List<Equipment>>("none", "equipment", $"exercise={Exercise.Id}");
        }

        private async void OnEditButtonClicked(object sender, EventArgs e) => await NavigationService.GoToAsync(nameof(ExerciseCreationPage), new KeyValuePair<string, object>("edit", Exercise));

        private void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            confirmHandler = async choice =>
            {
                if (!choice)
                    return;

                await deleteService.Delete(Exercise);
                await NavigationService.GoToAsync("..");
            };
            OpenConfirmDialog("Are you sure you want to delete this exercise?");
        }

        private async void OnExerciseSelect(object sender, EventArgs e) => await NavigationService.GoToAsync("../..", new KeyValuePair<string, object>("selectedExercise", Exercise));

        #region Search
        private async void OnPrimaryMuscleGroupSearch(object sender, SelectionChangedEventArgs e)
        {
            if (isInSelectionMode)
                return;

            await NavigationService.SearchAsync($"primarymusclegroup={(e.CurrentSelection[0] as MuscleGroupDisplay).Id}");
        }
        private async void OnPrimaryMuscleSearch(object sender, SelectionChangedEventArgs e)
        {
            if (isInSelectionMode)
                return;

            await NavigationService.SearchAsync($"primarymuscle={(e.CurrentSelection[0] as Muscle).Id}");
        }
        private async void OnEquipmentSearch(object sender, SelectionChangedEventArgs e)
        {
            if (isInSelectionMode)
                return;

            await NavigationService.SearchAsync($"equipment={(e.CurrentSelection[0] as Equipment).Id}");
        }
        #endregion

        #region Confirm Dialog
        private void OnWhiteOverlayClicked(object sender, EventArgs e) => CloseConfirmDialog();
        private void OnCancelClicked(object sender, EventArgs e) => HandleConfirmDialog(false);
        private void OnYesClicked(object sender, EventArgs e) => HandleConfirmDialog(true);
        private void OpenConfirmDialog(string message)
        {
            confirmDialogMessage.Text = message;
            confirmDialogWrapper.IsVisible = true;
            whiteOverlay.IsVisible = true;
        }
        private void CloseConfirmDialog()
        {
            confirmDialogWrapper.IsVisible = false;
            whiteOverlay.IsVisible = false;
        }
        private void HandleConfirmDialog(bool choice)
        {
            try
            {
                confirmHandler(choice);
                CloseConfirmDialog();
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
            }
        }
        #endregion
    }
}