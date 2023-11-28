using ProjectGym.Services.DatabaseSerialization;

namespace ProjectGym.Models
{
    public class Muscle
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;

        public MuscleGroup MuscleGroup { get; set; } = null!;
        [ModelReference("MuscleGroup")]
        public int MuscleGroupId { get; set; }
    }
}