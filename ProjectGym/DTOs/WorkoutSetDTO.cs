namespace ProjectGym.DTOs
{
    public class WorkoutSetDTO
    {
        public Guid Id { get; set; }
        public int TargetSets { get; set; }
        public bool DropSets { get; set; } = false;

        public Guid WorkoutId { get; set; }
        public Guid SetId { get; set; }
        public Guid? SuperSetId { get; set; }
    }
}
