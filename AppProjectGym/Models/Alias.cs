namespace AppProjectGym.Models
{
    public class ExerciseAlias
    {
        public int Id { get; set; }
        public string Alias { get; set; } = "";
        public int ExerciseId { get; set; }
    }
}
