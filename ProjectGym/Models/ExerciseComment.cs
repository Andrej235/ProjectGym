namespace ProjectGym.Models
{
    public class ExerciseComment
    {
        public int Id { get; set; }
        public string Comment { get; set; } = null!;

        public Exercise Exercise { get; set; } = null!;
        public int ExerciseId { get; set; }
    }
}