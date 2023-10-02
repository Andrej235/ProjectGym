namespace ProjectGym.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string UUID { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ExerciseBaseUUID { get; set; } = null!;
        public string Description { get; set; } = null!;

        public IEnumerable<Exercise> VariationExercise { get; set; } = new List<Exercise>();

        public ExerciseCategory Category { get; set; } = null!;
        public int CategoryId { get; set; }

        public IEnumerable<Muscle> Muscles { get; set; } = new List<Muscle>();
        public IEnumerable<Muscle> MusclesSecondary { get; set; } = new List<Muscle>();
        public IEnumerable<Equipment> Equipment { get; set; } = new List<Equipment>();

        public IEnumerable<ExerciseImage> Images { get; set; } = new List<ExerciseImage>();
        public IEnumerable<ExerciseVideo> Videos { get; set; } = new List<ExerciseVideo>();

        public IEnumerable<ExerciseComment> Comments { get; set; } = new List<ExerciseComment>();
        public IEnumerable<ExerciseNote> Notes { get; set; } = new List<ExerciseNote>();
        public IEnumerable<ExerciseAlias> Aliases { get; set; } = new List<ExerciseAlias>();
    }
}