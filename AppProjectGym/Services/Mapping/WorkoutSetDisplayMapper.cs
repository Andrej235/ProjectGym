using AppProjectGym.Information;
using AppProjectGym.Models;
using AppProjectGym.Services.Read;

namespace AppProjectGym.Services.Mapping
{
    public class WorkoutSetDisplayMapper(IReadService readService, IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper) : IEntityDisplayMapper<WorkoutSet, WorkoutSetDisplay>
    {
        public async Task<WorkoutSetDisplay> Map(WorkoutSet entity)
        {
            var setDisplay = new SetDisplay { Set = await readService.Get<Set>("none", $"set/{entity.SetId}") };
            setDisplay.Exercise = await exerciseDisplayMapper.Map(await readService.Get<Exercise>("images", $"exercise/{setDisplay.Set.ExerciseId}"));
            setDisplay.Weight = await readService.Get<PersonalExerciseWeight>("none", ReadService.TranslateEndPoint("weight", 0, 1), $"exercise={setDisplay.Exercise.Exercise.Id}", $"user={ClientInfo.User.Id}", "current=true") ?? new();
            
            WorkoutSetDisplay workoutSetDisplay = new()
            {
                Id = entity.Id,
                TargetSets = entity.TargetSets,
                Set = setDisplay,
            };

            return workoutSetDisplay;
        }
    }
}
