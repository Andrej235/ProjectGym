namespace AppProjectGym.Models
{
    public class ExerciseNote
    {
        public int Id { get; set; }
        public string Comment { get; set; } = null!;
        public int ExerciseId { get; set; }
    }
}
