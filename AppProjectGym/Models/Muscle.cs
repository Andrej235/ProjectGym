namespace AppProjectGym.Models
{
    public class Muscle
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int MuscleGroupId { get; set; }
        public IEnumerable<int> PrimaryInExercises { get; set; } = new List<int>();
        public IEnumerable<int> SecondaryInExercises { get; set; } = new List<int>();
    }
}