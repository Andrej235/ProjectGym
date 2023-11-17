namespace ProjectGym.Models
{
    public class CommentUpvote
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public Guid UserId { get; set; }
    }
}
