using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class ImageCreateService(IReadService<Exercise> exerciseReadService, ExerciseContext context) : ICreateService<Image, int>
    {
        public async Task<int> Add(Image toAdd)
        {
            if (toAdd.ExerciseId < 1)
                return default;

            try
            {
                await exerciseReadService.Get(x => x.Id == toAdd.ExerciseId, "none");

                await context.Images.AddAsync(toAdd);
                await context.SaveChangesAsync();

                return toAdd.Id;
            }
            catch (NullReferenceException)
            {

                Debug.WriteLine($"Exercise with Id {toAdd.ExerciseId} was not found");
                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while trying to add Image: {ex.Message}");
                return default;
            }
        }
    }
}
