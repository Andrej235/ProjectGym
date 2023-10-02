namespace ProjectGym.Models
{
    public class ExerciseAlias
    {
        public int Id { get; set; }
        public string Alias { get; set; } = null!;
        public int ExerciseId { get; set; }
    }
}