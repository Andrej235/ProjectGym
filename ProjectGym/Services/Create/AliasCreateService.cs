using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Services.Create
{
    public class AliasCreateService(ExerciseContext context, IReadService<Exercise> exerciseReadService) : ICreateService<Alias>
    {
        public async Task<object> Add(Alias toAdd)
        {
            if (toAdd.ExerciseId < 1)
                return default(int);

            try
            {
                await exerciseReadService.Get(x => x.Id == toAdd.ExerciseId, "none");

                await context.Aliases.AddAsync(toAdd);
                await context.SaveChangesAsync();

                return toAdd.Id;
            }
            catch (NullReferenceException)
            {
                LogDebugger.LogError(new NullReferenceException($"Exercise with Id {toAdd.ExerciseId} was not found"));
                return default(int);
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return default(int);
            }
        }
    }
}
