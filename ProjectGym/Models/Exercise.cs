namespace ProjectGym.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public IEnumerable<MuscleGroup> PrimaryMuscleGroups { get; set; } = new List<MuscleGroup>();
        public IEnumerable<MuscleGroup> SecondaryMuscleGroups { get; set; } = new List<MuscleGroup>();
        public IEnumerable<Equipment> Equipment { get; set; } = new List<Equipment>();

        public IEnumerable<Image> Images { get; set; } = new List<Image>();

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<Note> Notes { get; set; } = new List<Note>();
        public IEnumerable<Alias> Aliases { get; set; } = new List<Alias>();
    }
}