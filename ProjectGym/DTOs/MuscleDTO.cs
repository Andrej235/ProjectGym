
namespace ProjectGym.DTOs
{
    public class MuscleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public int MuscleGroupId { get; set; }
    }
}
