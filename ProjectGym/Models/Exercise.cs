namespace ProjectGym.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string UUID { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int ExerciseBase { get; set; }
        public string ExerciseBaseUUID { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Category { get; set; }
        public IEnumerable<int> Muscles { get; set; } = new List<int>();
        public IEnumerable<int> MusclesSecondary { get; set; } = new List<int>();
        public IEnumerable<int> Equipment { get; set; } = new List<int>();
        public IEnumerable<int> CommentIds { get; set; } = new List<int>();
        public IEnumerable<int> ImageIds { get; set; } = new List<int>();
        public IEnumerable<int> VariationExerciseIds { get; set; } = new List<int>();
        public IEnumerable<int> VideoIds { get; set; } = new List<int>();
        public IEnumerable<int> NoteIds { get; set; } = new List<int>();
        public IEnumerable<int> AliaseIds { get; set; } = new List<int>();
    }
}