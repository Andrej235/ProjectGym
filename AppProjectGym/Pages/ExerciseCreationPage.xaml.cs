using AppProjectGym.Models;
using AppProjectGym.Services;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Delete;
using AppProjectGym.Services.Read;
using AppProjectGym.Services.Update;
using AppProjectGym.Utilities;
using System.Reflection;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Pages
{
    public partial class ExerciseCreationPage : ContentPage, IQueryAttributable
    {
        private readonly IReadService readService;
        private readonly ICreateService createService;
        private readonly IUpdateService updateService;
        private readonly IDeleteService deleteService;
        private bool isEditing;

        public ExerciseCreationPage(IReadService readService, ICreateService createService, IUpdateService updateService, IDeleteService deleteService)
        {
            InitializeComponent();
            BindingContext = this;

            this.readService = readService;
            this.createService = createService;
            this.updateService = updateService;
            this.deleteService = deleteService;
        }

        private List<MuscleGroup> muscleGroups;
        private List<Muscle> muscles;

        private List<MuscleGroupDisplay> muscleGroupDisplays;

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

        private Exercise exercise;

        public List<MuscleGroupRepresentation> PrimaryMuscleGroupRepresentations
        {
            get => primaryMuscleGroupRepresentations;

            set
            {
                primaryMuscleGroupRepresentations = value;
                OnPropertyChanged();
            }
        }
        private List<MuscleGroupRepresentation> primaryMuscleGroupRepresentations;

        public List<MuscleGroupRepresentation> SecondaryMuscleGroupRepresentations
        {
            get => secondaryMuscleGroupRepresentations;
            set
            {
                secondaryMuscleGroupRepresentations = value;
                OnPropertyChanged();
            }
        }
        private List<MuscleGroupRepresentation> secondaryMuscleGroupRepresentations;

        public List<Image> Images
        {
            get => images;
            set
            {
                images = value;
                OnPropertyChanged();
            }
        }
        private List<Image> images;
        private IEnumerable<Image> originalImages;

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
        private IEnumerable<ExerciseNote> originalNotes;

        public List<ExerciseAlias> Aliases
        {
            get => aliases;
            set
            {
                aliases = value;
                OnPropertyChanged();
            }
        }
        private List<ExerciseAlias> aliases;
        private IEnumerable<ExerciseAlias> originalAliases;



        private async void OnSaveExercise(object sender, EventArgs e)
        {
            if (isEditing)
                await UpdateExercise();
            else
                await CreateExercise();
        }

        public async Task UpdateExercise()
        {
            var selectedEquipment = equipmentSelector.SelectedItems.Cast<Equipment>();
            var selectedPrimaryMuscleGroupRepresentatins = primaryMuscleGroupSelector.SelectedItems.Cast<MuscleGroupRepresentation>();
            var selectedSecondaryMuscleGroupRepresentatins = secondaryMuscleGroupSelector.SelectedItems.Cast<MuscleGroupRepresentation>();

            //exercise.Name = nameInput.Text; //Name doesn't work?
            exercise.Description = descriptionInput.Text;
            exercise.EquipmentIds = selectedEquipment.Select(x => x.Id);
            exercise.PrimaryMuscleGroupIds = selectedPrimaryMuscleGroupRepresentatins.Select(x => x.MuscleGroupDisplay.Id);
            exercise.PrimaryMuscleIds = selectedPrimaryMuscleGroupRepresentatins.SelectMany(x => x.SelectedMuscles).Select(x => x.Id);
            exercise.SecondaryMuscleGroupIds = selectedSecondaryMuscleGroupRepresentatins.Select(x => x.MuscleGroupDisplay.Id);
            exercise.SecondaryMuscleIds = selectedSecondaryMuscleGroupRepresentatins.SelectMany(x => x.SelectedMuscles).Select(x => x.Id);

            var wasUpdateSuccessful = await updateService.Update(exercise);
            if (!wasUpdateSuccessful)
            {
                await NavigationService.GoToAsync("..");
                return;
            }

            await SaveListPropertyChangesToDB(Images, originalImages, "image");
            await SaveListPropertyChangesToDB(Notes, originalNotes, "note");
            await SaveListPropertyChangesToDB(Aliases, originalAliases, "alias");

            await NavigationService.GoToAsync("..");
        }

        public async Task CreateExercise()
        {
            exercise = new();
            var selectedEquipment = equipmentSelector.SelectedItems.Cast<Equipment>();
            var selectedPrimaryMuscleGroupRepresentatins = primaryMuscleGroupSelector.SelectedItems.Cast<MuscleGroupRepresentation>();
            var selectedSecondaryMuscleGroupRepresentatins = secondaryMuscleGroupSelector.SelectedItems.Cast<MuscleGroupRepresentation>();

            exercise.Name = nameInput.Text;
            exercise.Description = descriptionInput.Text;
            exercise.EquipmentIds = selectedEquipment.Select(x => x.Id);
            exercise.PrimaryMuscleGroupIds = selectedPrimaryMuscleGroupRepresentatins.Select(x => x.MuscleGroupDisplay.Id);
            exercise.PrimaryMuscleIds = selectedPrimaryMuscleGroupRepresentatins.SelectMany(x => x.SelectedMuscles).Select(x => x.Id);
            exercise.SecondaryMuscleGroupIds = selectedSecondaryMuscleGroupRepresentatins.Select(x => x.MuscleGroupDisplay.Id);
            exercise.SecondaryMuscleIds = selectedSecondaryMuscleGroupRepresentatins.SelectMany(x => x.SelectedMuscles).Select(x => x.Id);

            try
            {
                string newExerciseId = await createService.Add(exercise);
                if (newExerciseId == "")
                    throw new Exception("Newly created exercise doesn't have an id");

                exercise.Id = Convert.ToInt32(newExerciseId);

                await SaveListPropertyChangesToDB(Images, originalImages, "image");
                await SaveListPropertyChangesToDB(Notes, originalNotes, "note");
                await SaveListPropertyChangesToDB(Aliases, originalAliases, "alias");

                await NavigationService.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await NavigationService.GoToAsync("..");
                LogDebugger.LogError(ex);
                return;
            }
        }

        private async Task SaveListPropertyChangesToDB<T>(IEnumerable<T> entities, IEnumerable<T> originalEntities, string endPoint = "") where T : class
        {
            try
            {
                var idProperty = typeof(T).GetProperty("Id");
                var entitiesToAdd = entities.Where(x => Convert.ToInt32(idProperty.GetValue(x)) == 0);
                var entitiesToDelete = originalEntities.Where(x => Convert.ToInt32(idProperty.GetValue(x)) != 0).Except(entities);

                var exerciseIdProperty = typeof(T).GetProperty("ExerciseId");
                if (exerciseIdProperty != null)
                {
                    if (exercise.Id <= 0)
                        return;

                    foreach (var entity in entitiesToAdd)
                        exerciseIdProperty.SetValue(entity, exercise.Id);

                    foreach (var entity in entitiesToDelete)
                        exerciseIdProperty.SetValue(entity, exercise.Id);
                }

                foreach (var entity in entitiesToAdd)
                    await createService.Add(entity, endPoint);

                foreach (var entity in entitiesToDelete)
                    await deleteService.Delete(entity, endPoint);
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return;
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            muscleGroups = await readService.Get<List<MuscleGroup>>("all");
            muscles = await readService.Get<List<Muscle>>("all");
            Equipment = await readService.Get<List<Equipment>>("all");

            muscleGroupDisplays = muscleGroups.Select(x => new MuscleGroupDisplay()
            {
                Id = x.Id,
                Name = x.Name,
                Muscles = muscles.Where(y => y.MuscleGroupId == x.Id)
            }).ToList();

            PrimaryMuscleGroupRepresentations = muscleGroupDisplays.Select(x => new MuscleGroupRepresentation(x, [])).ToList();
            SecondaryMuscleGroupRepresentations = muscleGroupDisplays.Select(x => new MuscleGroupRepresentation(x, [])).ToList();

            Images ??= [];
            Notes ??= [];
            Aliases ??= [];

            if (!query.TryGetValue("edit", out object value))
            {
                isEditing = false;
                return;
            }

            isEditing = true;
            exercise = value as Exercise;

            nameInput.Text = exercise.Name;
            descriptionInput.Text = exercise.Description;

            Images = await readService.Get<List<Image>>("none", "image", $"exercise={exercise.Id}");
            originalImages = Images.ToList();

            Notes = await readService.Get<List<ExerciseNote>>("none", "note", $"exercise={exercise.Id}");
            originalNotes = Notes.ToList();

            Aliases = await readService.Get<List<ExerciseAlias>>("none", "alias", $"exercise={exercise.Id}");
            originalAliases = Aliases.ToList();

            foreach (var muscle in muscles)
            {
                if (muscle.PrimaryInExercises.Contains(exercise.Id))
                    PrimaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay.Muscles.Contains(muscle)).SelectedMuscles.Add(muscle);

                if (muscle.SecondaryInExercises.Contains(exercise.Id))
                    SecondaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay.Muscles.Contains(muscle)).SelectedMuscles.Add(muscle);
            }

            equipmentSelector.SelectedItems = Equipment.Where(x => x.UsedInExerciseIds.Contains(exercise.Id)).Cast<object>().ToList();

            primaryMuscleGroupSelector.ItemsSource = null;
            primaryMuscleGroupSelector.ItemsSource = primaryMuscleGroupRepresentations;
            primaryMuscleGroupSelector.SelectedItems = PrimaryMuscleGroupRepresentations.Where(x => x.SelectedMuscles.Count > 0).Cast<object>().ToList();

            secondaryMuscleGroupSelector.ItemsSource = null;
            secondaryMuscleGroupSelector.ItemsSource = secondaryMuscleGroupRepresentations;
            secondaryMuscleGroupSelector.SelectedItems = SecondaryMuscleGroupRepresentations.Where(x => x.SelectedMuscles.Count > 0).Cast<object>().ToList();
        }

        private void OnMuscleSelect(object sender, SelectionChangedEventArgs e)
        {
            if (sender is CollectionView innerCollection && innerCollection.ItemsSource is IEnumerable<Muscle> musclesSource)
            {
                if (innerCollection.Parent.Parent.Parent is CollectionView outerCollection)
                {
                    MuscleGroupDisplay display = muscleGroupDisplays.First(x => x.Id == musclesSource.First().MuscleGroupId);
                    IEnumerable<Muscle> selectedMuscles = innerCollection.SelectedItems.Cast<Muscle>();
                    if (outerCollection == primaryMuscleGroupSelector)
                    {
                        var muscleGroupRepresentation = PrimaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay == display);
                        var oldSelectedMuscles = muscleGroupRepresentation.SelectedMuscles;
                        muscleGroupRepresentation.SelectedMuscles.AddRange(selectedMuscles.Where(muscle => !oldSelectedMuscles.Contains(muscle)));

                        var musclesToDelete = oldSelectedMuscles.Where(muscle => !selectedMuscles.Contains(muscle)).ToList();
                        for (int i = 0; i < musclesToDelete.Count; i++)
                            PrimaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay == display).SelectedMuscles.Remove(musclesToDelete[i]);

                        if (muscleGroupRepresentation.SelectedMuscles.Count > 0)
                        {
                            if (!outerCollection.SelectedItems.Contains(muscleGroupRepresentation))
                                outerCollection.SelectedItems.Add(muscleGroupRepresentation);
                        }
                        else if (outerCollection.SelectedItems.Contains(muscleGroupRepresentation))
                        {
                            outerCollection.SelectedItems.Remove(muscleGroupRepresentation);
                        }
                    }
                    else if (outerCollection == secondaryMuscleGroupSelector)
                    {
                        var muscleGroupRepresentation = secondaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay == display);
                        var oldSelectedMuscles = muscleGroupRepresentation.SelectedMuscles;
                        muscleGroupRepresentation.SelectedMuscles.AddRange(selectedMuscles.Where(muscle => !oldSelectedMuscles.Contains(muscle)));

                        var musclesToDelete = oldSelectedMuscles.Where(muscle => !selectedMuscles.Contains(muscle)).ToList();
                        for (int i = 0; i < musclesToDelete.Count; i++)
                            secondaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay == display).SelectedMuscles.Remove(musclesToDelete[i]);

                        if (muscleGroupRepresentation.SelectedMuscles.Count > 0)
                        {
                            if (!outerCollection.SelectedItems.Contains(muscleGroupRepresentation))
                                outerCollection.SelectedItems.Add(muscleGroupRepresentation);
                        }
                        else if (outerCollection.SelectedItems.Contains(muscleGroupRepresentation))
                        {
                            outerCollection.SelectedItems.Remove(muscleGroupRepresentation);
                        }
                    }
                }
            }
        }

        private void OnAddNewImage(object sender, EventArgs e)
        {
            string enteredUrl = imageURLInput.Text;

            if (enteredUrl == "")
                return;

            Images.Add(new() { ImageURL = enteredUrl });
            imageURLInput.Text = "";

            imagesCollection.ItemsSource = null;
            imagesCollection.ItemsSource = Images;
        }

        private void OnImageDeleted(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem)
            {
                if (swipeItem.BindingContext is string url)
                {
                    Image image = Images.FirstOrDefault(x => x.ImageURL == url);
                    if (image is null)
                        return;

                    Images.Remove(image);
                    imagesCollection.ItemsSource = null;
                    imagesCollection.ItemsSource = Images;
                }
            }
        }

        private void OnAddNewNote(object sender, EventArgs e)
        {
            string enteredNote = noteTextInput.Text;

            if (enteredNote == "")
                return;

            Notes.Add(new() { Note = enteredNote });
            noteTextInput.Text = "";

            notesCollection.ItemsSource = null;
            notesCollection.ItemsSource = Notes;
        }

        private void OnNoteDeleted(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem)
            {
                if (swipeItem.BindingContext is string noteText)
                {
                    var note = Notes.FirstOrDefault(x => x.Note == noteText);
                    if (note is null)
                        return;

                    Notes.Remove(note);
                    notesCollection.ItemsSource = null;
                    notesCollection.ItemsSource = Notes;
                }
            }
        }

        private void OnAddNewAlias(object sender, EventArgs e)
        {
            string enteredAlias = aliasInput.Text;

            if (enteredAlias == "")
                return;

            Aliases.Add(new() { Alias = enteredAlias });
            aliasInput.Text = "";

            aliasCollection.ItemsSource = null;
            aliasCollection.ItemsSource = Aliases;
        }

        private void OnAliasDeleted(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem)
            {
                if (swipeItem.BindingContext is string aliasText)
                {
                    var alias = Aliases.FirstOrDefault(x => x.Alias == aliasText);
                    if (alias is null)
                        return;

                    Aliases.Remove(alias);
                    aliasCollection.ItemsSource = null;
                    aliasCollection.ItemsSource = Aliases;
                }
            }
        }
    }
}
