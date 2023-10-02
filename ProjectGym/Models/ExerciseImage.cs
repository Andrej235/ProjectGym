namespace ProjectGym.Models
{
    public class ExerciseImage
    {
        public int Id { get; set; }
        public string UUID { get; set; } = null!;
        public int ExerciseId { get; set; }
        public string ExerciseUUID { get; set; } = null!;
        public string ImageURL { get; set; } = null!;
        public bool IsMain { get; set; }
        public string Style { get; set; } = null!;
    }
}