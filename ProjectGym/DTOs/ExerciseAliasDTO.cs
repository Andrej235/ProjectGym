namespace ProjectGym.DTOs
{
    public class ExerciseAliasDTO
    {
        public int Id { get; set; }
        public string Alias { get; set; } = null!;
        public int ExerciseId { get; set; }
    }
}
