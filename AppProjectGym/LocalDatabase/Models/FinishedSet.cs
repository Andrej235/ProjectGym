using LocalJSONDatabase.Attributes;

namespace AppProjectGym.LocalDatabase.Models
{
    public class FinishedSet
    {
        [PrimaryKey]
        public Guid SetId { get; set; }
        public int Time { get; set; }
        public int Reps { get; set; }
        public float Weight { get; set; }

        //[ForeignKey]
        public FinishedWorkoutSet WorkoutSet { get; set; }
    }
}
