using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Services.Create
{
    public class SecondaryMuscleExerciseConnectionCreateService(ExerciseContext context, IReadService<SecondaryMuscleGroupInExercise> readService) : ICreateService<SecondaryMuscleGroupInExercise>
    {
        public async Task<object> Add(SecondaryMuscleGroupInExercise toAdd)
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
                    await context.SecondaryMuscleGroups.AddAsync(toAdd);
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
