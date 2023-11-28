using ProjectGym.Services.DatabaseSerialization;

namespace ProjectGym.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageURL { get; set; } = null!;

        public Exercise Exercise { get; set; } = null!;
        [ModelReference("Exercise")]
        public int ExerciseId { get; set; }
    }
}