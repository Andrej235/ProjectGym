namespace AppProjectGym.Models
{
    public class StartedWorkout_SetDisplay
    {
        public WorkoutSetDisplay WorkoutSet { get; set; }
        public List<FinishedSetDisplay> FinishedSets { get; set; }
    }
}
