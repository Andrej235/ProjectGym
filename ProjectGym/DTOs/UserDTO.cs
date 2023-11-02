namespace ProjectGym.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;

        public IEnumerable<Guid> WeightIds { get; set; } = new List<Guid>();
        public IEnumerable<Guid> CreatedWorkoutIds { get; set; } = new List<Guid>();
        public IEnumerable<Guid> CreatedExerciseSetIds { get; set; } = new List<Guid>();

        public IEnumerable<int> ExerciseCommentIds { get; set; } = new List<int>();
        public IEnumerable<int> ExerciseCommentUpvoteIds { get; set; } = new List<int>();
        public IEnumerable<int> ExerciseCommentDownvoteIds { get; set; } = new List<int>();
        public IEnumerable<int> ExerciseBookmarkIds { get; set; } = new List<int>();
    }
}
