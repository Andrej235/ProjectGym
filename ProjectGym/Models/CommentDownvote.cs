using ProjectGym.Services.DatabaseSerialization;

namespace ProjectGym.Models
{
    public class CommentDownvote
    {
        public int Id { get; set; }
        [ModelReference("Comment")]
        public int CommentId { get; set; }
        [ModelReference("User")]
        public Guid UserId { get; set; }
    }
}
