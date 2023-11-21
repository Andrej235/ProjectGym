using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Utilities;

namespace ProjectGym.Services.Update
{
    public class WeightUpdateService(ExerciseContext context) : IUpdateService<PersonalExerciseWeight>
    {
        public async Task Update(PersonalExerciseWeight updatedEntity)
        {
            try
            {
                context.Update(updatedEntity);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
            }
        }
    }
}
