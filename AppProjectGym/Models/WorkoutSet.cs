namespace AppProjectGym.Models
{
    public class WorkoutSet
    {
        public Guid Id { get; set; }
        public int TargetSets { get; set; }

        public Guid WorkoutId { get; set; }
        public Guid SetId { get; set; }
        public Guid? SuperSetId { get; set; }
    }
}
