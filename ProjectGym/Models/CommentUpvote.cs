using ProjectGym.Services.DatabaseSerialization;

namespace ProjectGym.Models
{
    public class CommentUpvote
    {
        public int Id { get; set; }
        [ModelReference("Comment")]
        public int CommentId { get; set; }
        [ModelReference("User")]
        public Guid UserId { get; set; }
    }
}
