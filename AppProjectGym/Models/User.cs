using System.Diagnostics.CodeAnalysis;

namespace AppProjectGym.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public IEnumerable<Guid> WeightIds { get; set; } = new List<Guid>();
        public IEnumerable<Guid> CreatedWorkoutIds { get; set; } = new List<Guid>();
        public IEnumerable<Guid> CreatedExerciseSetIds { get; set; } = new List<Guid>();

        public IEnumerable<int> CommentIds { get; set; } = new List<int>();
        public IEnumerable<int> CommentUpvoteIds { get; set; } = new List<int>();
        public IEnumerable<int> CommentDownvoteIds { get; set; } = new List<int>();
        public List<int> BookmarkIds { get; set; } = new List<int>();
    }
}
