using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class PrimaryMuscleExerciseConnectionCreateService(ExerciseContext context, IReadService<PrimaryMuscleGroupInExercise> readService) : ICreateService<PrimaryMuscleGroupInExercise, int>
    {
        public async Task<int> Add(PrimaryMuscleGroupInExercise toAdd)
        {
            try
            {
                await readService.Get(x => x.MuscleGroupId == toAdd.MuscleGroupId && x.ExerciseId == toAdd.ExerciseId, "none");
                return default;
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
                    Debug.WriteLine($"Error occured while trying to add PrimaryMuscleExerciseConnection: {ex.Message}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while trying to add PrimaryMuscleExerciseConnection: {ex.Message}");
                return default;
            }
        }
    }
}
