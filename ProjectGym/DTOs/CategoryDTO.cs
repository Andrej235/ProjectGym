namespace ProjectGym.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IEnumerable<int> ExerciseIds { get; set; } = new List<int>();
    }
}
