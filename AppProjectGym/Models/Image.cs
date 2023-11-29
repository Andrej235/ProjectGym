namespace AppProjectGym.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageURL { get; set; } = null!;
        public int ExerciseId { get; set; }
    }
}
