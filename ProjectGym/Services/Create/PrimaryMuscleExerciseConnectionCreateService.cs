using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class PrimaryMuscleExerciseConnectionCreateService : ICreateService<PrimaryMuscleExerciseConnection>
    {
        private readonly ExerciseContext context;
        private readonly IReadService<PrimaryMuscleExerciseConnection> readService;
        public PrimaryMuscleExerciseConnectionCreateService(ExerciseContext context, IReadService<PrimaryMuscleExerciseConnection> readService)
        {
            this.context = context;
            this.readService = readService;
        }

        public async Task<bool> Add(PrimaryMuscleExerciseConnection toAdd)
        {
            try
            {
                await readService.Get(x => x.MuscleId == toAdd.MuscleId && x.ExerciseId == toAdd.ExerciseId, "none");
                return false;
            }
            catch (NullReferenceException)
            {
                try
                {
                    await context.PrimaryMuscleExerciseConnections.AddAsync(toAdd);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error occured while trying to add PrimaryMuscleExerciseConnection: {ex.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while trying to add PrimaryMuscleExerciseConnection: {ex.Message}");
                return false;
            }
        }
    }
}
