namespace ProjectGym.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<Exercise> UsedInExercises { get; set; } = Enumerable.Empty<Exercise>();
    }
}