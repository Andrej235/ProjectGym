using LocalJSONDatabase.Attributes;

namespace AppProjectGym.LocalDatabase.Models
{
    public class FinishedWorkout
    {
        [PrimaryKey]
        public Guid WorkoutId { get; set; }
        public Guid CreatorId { get; set; }
        public IEnumerable<Guid> WorkoutSetIds { get; set; } = new List<Guid>();

        [ForeignKey]
        public IEnumerable<FinishedWorkoutSet> WorkoutSets { get; set; }
    }
}
