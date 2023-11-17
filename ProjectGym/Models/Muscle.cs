namespace ProjectGym.Models
{
    public class Muscle
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;

        public int MuscleGroupId { get; set; }
        public MuscleGroup MuscleGroup { get; set; } = null!;
    }
}