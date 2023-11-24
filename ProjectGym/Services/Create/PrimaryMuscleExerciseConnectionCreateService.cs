using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class PrimaryMuscleExerciseConnectionCreateService(ExerciseContext context, IReadService<PrimaryMuscleGroupInExercise> readService) : ICreateService<PrimaryMuscleGroupInExercise>
    {
        public async Task<object> Add(PrimaryMuscleGroupInExercise toAdd)
        {
            try
            {
                await readService.Get(x => x.MuscleGroupId == toAdd.MuscleGroupId && x.ExerciseId == toAdd.ExerciseId, "none");
                throw new Exception("Entity already exists");
            }
            catch (NullReferenceException)
            {
                try
                {
                    await context.PrimaryMuscleGroups.AddAsync(toAdd);
                    await context.SaveChangesAsync();
                    return toAdd.Id;
                }
                catch (Exception ex)
                {
                    LogDebugger.LogError(ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                throw;
            }
        }
    }
}
