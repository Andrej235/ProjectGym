namespace ProjectGym.Models
{
    public class MuscleGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<Muscle> Muscles { get; set; } = new List<Muscle>();
        public IEnumerable<Exercise> PrimaryInExercises { get; set; } = new List<Exercise>();
        public IEnumerable<Exercise> SecondaryInExercises { get; set; } = new List<Exercise>();
    }
}
