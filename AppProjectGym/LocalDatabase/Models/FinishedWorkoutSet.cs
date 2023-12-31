using LocalJSONDatabase.Attributes;

namespace AppProjectGym.LocalDatabase.Models
{
    public class FinishedWorkoutSet
    {
        [PrimaryKey]
        public Guid WorkoutSetId { get; set; }
        public int RestTimeAfterTheSet { get; set; }

        [ForeignKey]
        public FinishedSet Set { get; set; }

        [ForeignKey]
        public FinishedWorkout Workout { get; set; }
    }
}
