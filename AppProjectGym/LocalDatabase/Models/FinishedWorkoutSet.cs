using LocalJSONDatabase.Attributes;

namespace AppProjectGym.LocalDatabase.Models
{
    public class FinishedWorkoutSet
    {
        [PrimaryKey]
        public int Id { get; set; } 

        public Guid WorkoutSetId { get; set; }

        [ForeignKey]
        public IEnumerable<FinishedSet> Sets { get; set; } = [];

        [ForeignKey]
        public FinishedWorkout Workout { get; set; }
    }
}
