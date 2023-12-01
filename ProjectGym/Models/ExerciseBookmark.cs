using ProjectGym.Services.DatabaseSerialization;

namespace ProjectGym.Models
{
    public class ExerciseBookmark
    {
        public int Id { get; set; }
        [ModelReference("Exercise")]
        public int ExerciseId { get; set; }
        [ModelReference("User")]
        public Guid UserId { get; set; }
    }
}
