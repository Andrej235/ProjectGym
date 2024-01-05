namespace ProjectGym.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<MuscleGroup> PrimaryMuscleGroups { get; set; } = new List<MuscleGroup>();
        public IEnumerable<MuscleGroup> SecondaryMuscleGroups { get; set; } = new List<MuscleGroup>();
        public IEnumerable<Muscle> PrimaryMuscles { get; set; } = new List<Muscle>();
        public IEnumerable<Muscle> SecondaryMuscles { get; set; } = new List<Muscle>();
        public IEnumerable<Equipment> Equipment { get; set; } = new List<Equipment>();

        public IEnumerable<Image> Images { get; set; } = new List<Image>();

        public IEnumerable<Note> Notes { get; set; } = new List<Note>();
        public IEnumerable<Alias> Aliases { get; set; } = new List<Alias>();

        public IEnumerable<User> Bookmarks { get; set; } = new List<User>();
    }
}