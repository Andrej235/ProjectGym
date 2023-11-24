using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Services.Create
{
    public class ImageCreateService(IReadService<Exercise> exerciseReadService, ExerciseContext context) : ICreateService<Image>
    {
        public async Task<object> Add(Image toAdd)
        {
            if (toAdd.ExerciseId < 1)
                return default(int);

            try
            {
                await exerciseReadService.Get(x => x.Id == toAdd.ExerciseId, "none");

                await context.Images.AddAsync(toAdd);
                await context.SaveChangesAsync();

                return toAdd.Id;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                throw;
            }
        }
    }
}
