using Microsoft.IdentityModel.Tokens;

namespace ProjectGym.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = null!;

        public Exercise Exercise { get; set; } = null!;
        public int ExerciseId { get; set; }

        public User Creator { get; set; } = null!;
        public Guid CreatorId { get; set; }

        public IEnumerable<User> Upvotes { get; set; } = new List<User>();
        public IEnumerable<User> Downvotes { get; set; } = new List<User>();
    }
}