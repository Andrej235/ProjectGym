namespace ProjectGym.DTOs
{
    public class MuscleGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<int> MuscleIds { get; set; } = new List<int>();
        public IEnumerable<int> PrimaryInExercises { get; set; } = new List<int>();
        public IEnumerable<int> SecondaryInExercises { get; set; } = new List<int>();
    }
}
