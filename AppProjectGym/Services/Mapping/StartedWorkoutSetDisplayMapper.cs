using AppProjectGym.Models;

namespace AppProjectGym.Services.Mapping
{
    public class StartedWorkoutSetDisplayMapper(IEntityDisplayMapper<WorkoutSet, WorkoutSetDisplay> workoutSetDisplayMapper) : IEntityDisplayMapper<WorkoutSet, StartedWorkout_SetDisplay>
    {
        public async Task<StartedWorkout_SetDisplay> Map(WorkoutSet entity)
        {
            var workoutSetDisplay = await workoutSetDisplayMapper.Map(entity);
            var finishedSets = new List<FinishedSetDisplay>();
            for (int i = 0; i < entity.TargetSets; i++)
            {
                finishedSets.Add(new()
                {
                    FinishedReps = 0,
                    Time = 0,
                    Weight = null
                });
            }

            return new StartedWorkout_SetDisplay()
            {
                WorkoutSet = workoutSetDisplay,
                FinishedSets = finishedSets
            };
        }
    }
}
