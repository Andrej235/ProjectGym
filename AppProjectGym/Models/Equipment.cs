namespace AppProjectGym.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IEnumerable<int> UsedInExerciseIds { get; set; } = new List<int>();
    }
}
