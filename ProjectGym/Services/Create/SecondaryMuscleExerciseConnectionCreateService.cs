using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class SecondaryMuscleExerciseConnectionCreateService(ExerciseContext context, IReadService<SecondaryMuscleGroupInExercise> readService) : ICreateService<SecondaryMuscleGroupInExercise, int>
    {
        public async Task<int> Add(SecondaryMuscleGroupInExercise toAdd)
        {
            try
            {
                await readService.Get(x => x.MuscleGroupId == toAdd.MuscleGroupId&& x.ExerciseId == toAdd.ExerciseId, "none");
                return default;
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
                    Debug.WriteLine($"Error occured while trying to add SecondaryMuscleExerciseConnections: {ex.Message}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while trying to add SecondaryMuscleExerciseConnections: {ex.Message}");
                return default;
            }
        }
    }
}
