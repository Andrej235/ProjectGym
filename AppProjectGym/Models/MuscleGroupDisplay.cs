namespace AppProjectGym.Models
{
    public class MuscleGroupDisplay
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Muscle> Muscles { get; set; }
    }
}
