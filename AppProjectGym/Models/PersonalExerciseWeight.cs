namespace AppProjectGym.Models
{
    public class PersonalExerciseWeight
    {
        public float Weight { get; set; } = -1;
        public bool IsCurrent { get; set; }
        public DateTime? DateOfAchieving { get; set; }
        public int ExerciseId { get; set; }
        public Guid UserId { get; set; }
    }
}
