namespace ProjectGym.DTOs
{
    public class PersonalExerciseWeightDTO
    {
        public Guid Id { get; set; }
        public float Weight { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime? DateOfAchieving { get; set; }
        public int ExerciseId { get; set; }
        public Guid UserId { get; set; }
    }
}
