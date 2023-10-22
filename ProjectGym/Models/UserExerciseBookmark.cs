namespace ProjectGym.Models
{
    public class UserExerciseBookmark
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public Guid UserId { get; set; }
    }
}
