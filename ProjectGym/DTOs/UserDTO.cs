namespace ProjectGym.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public IEnumerable<Guid> WeightIds { get; set; } = new List<Guid>();
        public IEnumerable<Guid> CreatedWorkoutIds { get; set; } = new List<Guid>();
        public IEnumerable<Guid> CreatedExerciseSetIds { get; set; } = new List<Guid>();

        public IEnumerable<int> BookmarkIds { get; set; } = new List<int>();
    }
}
