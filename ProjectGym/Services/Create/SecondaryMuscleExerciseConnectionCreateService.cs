using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class SecondaryMuscleExerciseConnectionCreateService : ICreateService<SecondaryMuscleExerciseConnection>
    {
        private readonly ExerciseContext context;
        private readonly IReadService<SecondaryMuscleExerciseConnection> readService;
        public SecondaryMuscleExerciseConnectionCreateService(ExerciseContext context, IReadService<SecondaryMuscleExerciseConnection> readService)
        {
            this.context = context;
            this.readService = readService;
        }

        public async Task<bool> Add(SecondaryMuscleExerciseConnection toAdd)
        {
            try
            {
                await readService.Get(x => x.MuscleId == toAdd.MuscleId&& x.ExerciseId == toAdd.ExerciseId, "none");
                return false;
            }
            catch (NullReferenceException)
            {
                try
                {
                    await context.SecondaryMuscleExerciseConnections.AddAsync(toAdd);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error occured while trying to add SecondaryMuscleExerciseConnections: {ex.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while trying to add SecondaryMuscleExerciseConnections: {ex.Message}");
                return false;
            }
        }
    }
}
