using AppProjectGym.Charts;
using AppProjectGym.Information;
using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Delete;
using AppProjectGym.Services.Mapping;
using AppProjectGym.Services.Read;
using AppProjectGym.Utilities;

namespace AppProjectGym.Pages
{
    public partial class FullScreenExercisePage : ContentPage, IQueryAttributable
    {
        private delegate Task ConfirmHandler(bool input);
        private ConfirmHandler confirmHandler;

        private readonly IReadService readService;
        private readonly ICreateService createService;
        private readonly IDeleteService deleteService;
        private readonly IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper;

        public FullScreenExercisePage(IReadService readService, IDeleteService deleteService, IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper, ICreateService createService)
        {
            InitializeComponent();
            BindingContext = this;
            this.readService = readService;
            this.deleteService = deleteService;
            this.exerciseDisplayMapper = exerciseDisplayMapper;
            this.createService = createService;
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

            Exercise = await readService.Get<Exercise>("images", $"exercise/{id}");
            ExerciseDisplay = await exerciseDisplayMapper.Map(Exercise);

            IsBookmarked = ClientInfo.User.BookmarkIds.Contains(Exercise.Id);

            var currentWeight = await readService.Get<PersonalExerciseWeight>("none", "weight", $"user={ClientInfo.User.Id}", $"exercise={id}", "current=true");
            weightHistoryBtn.Text = currentWeight is null ? "Not yet attempted" : $"Weight you used last time: {currentWeight.Weight}KG";

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

        private async void OnOpenWeightHistory(object sender, EventArgs e)
        {
            if (sender is not Button button || button.Text == "Not yet attempted")
                return;

            isLoadingData = true;
            var weights = await readService.Get<List<PersonalExerciseWeight>>("none", ReadService.TranslateEndPoint("weight", 0, 20), $"exercise={Exercise.Id}", $"user={ClientInfo.User.Id}");
            if (weights.Count == 0)
            {
                isLoadingData = false;
                return;
            }

            if (weights.Count < 5)
            {
                weightHistoryLineChart.IsVisible = false;
            }
            else
            {
                weightHistoryLineChart.IsVisible = true;
                weightHistoryLineChart.Points = null;
                weightHistoryLineChart.Points = weights.Select(x => new ValuePoint(FormatDateTime(x.DateOfAchieving), x.Weight));
                static string FormatDateTime(DateTime? dateTime) => dateTime is null ? "" : $"{dateTime?.Day:D2}.{dateTime?.Month:D2}";
            }

            weightHistoryWrapper.IsVisible = true;

            var currentWeight = weights.LastOrDefault(x => x.IsCurrent);
            weightHistoryCurrentWeightLabel.Text = $"Current weight: {currentWeight.Weight}KG";
            weightHistoryCurrentWeightDateLabel.Text = $"Achieved: {currentWeight.DateOfAchieving?.ToString("dd.MM.yyyy.")}";

            var maxWeight = weights.MaxBy(x => x.Weight);
            weightHistoryMaxWeightLabel.Text = $"Maximum weight: {maxWeight.Weight}KG";
            weightHistoryMaxWeightDateLabel.Text = $"Achieved: {maxWeight.DateOfAchieving?.ToString("dd.MM.yyyy.")}";
            isLoadingData = false;
        }

        #region Custom back button logic
        protected override bool OnBackButtonPressed()
        {
            BackCommand.Execute(null);
            return true;
        }
        private bool isLoadingData;
        public Command BackCommand => new(() =>
        {
            if (isLoadingData)
                return;

            if (weightHistoryWrapper.IsVisible)
                weightHistoryWrapper.IsVisible = false;
            else
                GoBack();
        });
        private static async void GoBack() => await NavigationService.GoToAsync("..");
        #endregion

        #region Bookmarks
        public bool IsBookmarked
        {
            get => isBookmarked;
            set
            {
                isBookmarked = value;
                bookmarkBtn.Source = isBookmarked ? "bookmarkselected.png" : "bookmark.png";
                OnPropertyChanged();
            }
        }
        private bool isBookmarked;

        private async void OnBookmarkBtnClicked(object sender, EventArgs e)
        {
            //TODO: Make an endpoint on backend to see if an exercise is bookmarked and call it on appearing
            //Maybe add an endpoint for toggling bookmarks so the code on the frontend is simpler?
            var newBookmark = new Bookmark()
            {
                UserId = ClientInfo.User.Id,
                ExerciseId = Exercise.Id
            };
            var res = await createService.Add(newBookmark, "bookmark/toggle");
            if (bool.TryParse(res, out bool resIsBookmarked))
            {
                if (resIsBookmarked)
                    ClientInfo.User.BookmarkIds.Add(Exercise.Id);
                else
                    ClientInfo.User.BookmarkIds.Remove(Exercise.Id);

                IsBookmarked = resIsBookmarked;
            }
        }
        #endregion
    }
}
