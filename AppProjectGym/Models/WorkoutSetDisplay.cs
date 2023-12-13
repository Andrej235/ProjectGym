namespace AppProjectGym.Models
{
    public class WorkoutSetDisplay
    {
        public Guid Id { get; set; }
        public int TargetSets { get; set; }

        public SetDisplay Set { get; set; }
        public SetDisplay Superset { get; set; }
    }
}
