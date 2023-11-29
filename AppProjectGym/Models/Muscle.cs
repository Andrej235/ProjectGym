namespace AppProjectGym.Models
{
    public class Muscle
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = string.Empty;
        public int MuscleGroupId { get; set; }
    }
}