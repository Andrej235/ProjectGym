namespace ProjectGym.DTOs
{
    public class WorkoutSetDTO
    {
        public Guid Id { get; set; }
        public int TargetSets { get; set; }

        public Guid WorkoutId { get; set; }
        public Guid SetId { get; set; }
    }
}
