using AppProjectGym.Models;
using AppProjectGym.Services.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProjectGym.Services.Mapping
{
    public class WorkoutSetDisplayMapper(IReadService readService, IEntityDisplayMapper<Exercise, ExerciseDisplay> exerciseDisplayMapper) : IEntityDisplayMapper<WorkoutSet, WorkoutSetDisplay>
    {
        public async Task<WorkoutSetDisplay> Map(WorkoutSet entity)
        {
            var setDisplay = new SetDisplay { Set = await readService.Get<Set>("none", $"set/{entity.SetId}") };
            setDisplay.Exercise = await exerciseDisplayMapper.Map(await readService.Get<Exercise>("images", $"exercise/{setDisplay.Set.ExerciseId}"));

            WorkoutSetDisplay workoutSetDisplay = new()
            {
                Id = entity.Id,
                TargetSets = entity.TargetSets,
                Set = setDisplay,
            };

            if (entity.SuperSetId != null)
            {
                var supersetDisplay = new SetDisplay { Set = await readService.Get<Set>("none", $"set/{entity.SuperSetId}") };
                supersetDisplay.Exercise = await exerciseDisplayMapper.Map(await readService.Get<Exercise>("images", $"exercise/{supersetDisplay.Set.ExerciseId}"));
                workoutSetDisplay.Superset = supersetDisplay;
            }

            return workoutSetDisplay;
        }
    }
}
