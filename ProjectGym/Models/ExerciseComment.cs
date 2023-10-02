namespace ProjectGym.Models
{
    public class ExerciseComment
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public string Comment { get; set; } = null!;
    }
}