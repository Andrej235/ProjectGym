using AppProjectGym.Models;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Delete;
using AppProjectGym.Services.Read;
using AppProjectGym.Services.Update;
using System.Linq.Expressions;
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
        private List<Image> originalImages;

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
        private List<ExerciseNote> originalNotes;

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
        private List<ExerciseAlias> originalAliases;



        private async void OnCreateExercise(object sender, EventArgs e)
        {
            if (isEditing)
            {
                await UpdateExercise();
                return;
            }

            var name = nameInput.Text;
            var description = descriptionInput.Text;
            var imageURL = imageURLInput.Text;

            if (name is null ||
                name == "" ||
                description is null ||
                description == "" ||
                equipmentSelector.SelectedItems is null ||
                primaryMuscleGroupSelector.SelectedItems is null)
                return;

            Exercise exercise = new()
            {
                Name = name,
                Description = description,
                EquipmentIds = equipmentSelector.SelectedItems.Select(x => (x as Equipment).Id),
                PrimaryMuscleIds = primaryMuscleGroupSelector.SelectedItems.Select(x => (x as Muscle).Id),
                SecondaryMuscleIds = secondaryMuscleGroupSelector.SelectedItems is not null ? secondaryMuscleGroupSelector.SelectedItems.Select(x => (x as Muscle).Id) : new List<int>(),
                AliasIds = new List<int>(),
                ImageIds = new List<int>(),
                NoteIds = new List<int>(),
            };

            var exerciseId = await createService.Add(exercise);
            if (exerciseId == "")
                return;

            await createService.Add(new Image()
            {
                ImageURL = imageURL,
                ExerciseId = int.Parse(exerciseId),
            });
        }

        public async Task UpdateExercise()
        {
            //IT WORKSSS
            //*****************************************************************************************************
            /*            var imagesToAdd = Images.Where(x => x.Id == 0);
                        var imagesToDelete = originalImages.Where(x => !Images.Any(y => y.Id != 0 && y.ImageURL == x.ImageURL));
                        foreach (var img in imagesToAdd)
                            await createService.Add(img);

                        foreach (var img in imagesToDelete)
                            await deleteService.Delete(img);*/

            await SavePropertyChangesToDB(Images, originalImages, (x, y) => x.ImageURL == y.ImageURL, "image");
            await SavePropertyChangesToDB(Notes, originalNotes, (x, y) => x.Note == y.Note, "note");
            await SavePropertyChangesToDB(Aliases, originalAliases, (x, y) => x.Alias == y.Alias, "alias");
            //*****************************************************************************************************

            //await updateService.Update(exercise);
        }

        private async Task SavePropertyChangesToDB<T>(IEnumerable<T> entities, IEnumerable<T> originalEntities, Func<T, T, bool> equalExpression, string endPoint = "") where T : class
        {
            var idProperty = typeof(T).GetProperty("Id");
            var entitiesToAdd = entities.Where(x => Convert.ToInt32(idProperty.GetValue(x)) == 0);

            var entitiesToDelete = originalEntities.Where(x => !entities.Any(y => equalExpression(x, y)));
            //entitiesToDelete = entitiesToDelete.Where(x => Convert.ToInt32(idProperty.GetValue(x)) != 0);
            foreach (var img in entitiesToAdd)
                await createService.Add(img, endPoint);

            foreach (var img in entitiesToDelete)
                await deleteService.Delete(img, endPoint);
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
            originalImages = Images;

            Notes = await readService.Get<List<ExerciseNote>>("none", "note", $"exercise={exercise.Id}");
            originalNotes = Notes;

            Aliases = await readService.Get<List<ExerciseAlias>>("none", "alias", $"exercise={exercise.Id}");
            originalAliases = Aliases;

            PrimaryMuscleGroupRepresentations = muscleGroupDisplays.Select(x => new MuscleGroupRepresentation(x, [])).ToList();
            SecondaryMuscleGroupRepresentations = muscleGroupDisplays.Select(x => new MuscleGroupRepresentation(x, [])).ToList();
            foreach (var muscle in muscles)
            {
                if (muscle.PrimaryInExercises.Contains(exercise.Id))
                    PrimaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay.Muscles.Contains(muscle)).SelectedMuscles.Add(muscle);
                else if (muscle.SecondaryInExercises.Contains(exercise.Id))
                    SecondaryMuscleGroupRepresentations.First(x => x.MuscleGroupDisplay.Muscles.Contains(muscle)).SelectedMuscles.Add(muscle);
            }

            equipmentSelector.SelectedItems = Equipment.Where(x => x.UsedInExerciseIds.Contains(exercise.Id)).Cast<object>().ToList();
            primaryMuscleGroupSelector.SelectedItems = PrimaryMuscleGroupRepresentations.Where(x => x.SelectedMuscles.Count > 0).Cast<object>().ToList();
            secondaryMuscleGroupSelector.SelectedItems = SecondaryMuscleGroupRepresentations.Where(x => x.SelectedMuscles.Count > 0).Cast<object>().ToList();
        }

        private void OnMuscleSelect(object sender, SelectionChangedEventArgs e)
        {
            if (sender is CollectionView innerCollection && innerCollection.ItemsSource is IEnumerable<Muscle> musclesSource)
            {
                if (innerCollection.Parent.Parent is CollectionView outerCollection)
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

            Images.Add(new()
            {
                ImageURL = enteredUrl,
                ExerciseId = exercise.Id,
            });
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

            Notes.Add(new()
            {
                Note = enteredNote,
                ExerciseId = exercise.Id,
            });
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

            Aliases.Add(new()
            {
                Alias = enteredAlias,
                ExerciseId = exercise.Id,
            });
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
