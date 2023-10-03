namespace ProjectGym.Models
{
    public class ExerciseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}