using LocalJSONDatabase.Attributes;

namespace AppProjectGym.LocalDatabase.Models
{
    public class FinishedWorkout
    {
        [PrimaryKey]
        public int Id { get; set; } 

        public required Guid WorkoutId { get; set; }
        public DateTime DateTime { get; set; }

        [ForeignKey]
        public IEnumerable<FinishedWorkoutSet> WorkoutSets { get; set; }
    }
}
