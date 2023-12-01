namespace ProjectGym.DTOs
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string ImageURL { get; set; } = null!;
        public int ExerciseId { get; set; }
    }
}
