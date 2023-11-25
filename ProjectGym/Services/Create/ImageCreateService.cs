using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Services.Create
{
    public class ImageCreateService(IReadService<Exercise> exerciseReadService, ExerciseContext context) : CreateService<Image>(context)
    {
        protected override async Task<Exception?> IsEntityValid(Image entity)
        {
            try
            {
                if (entity.ExerciseId < 1)
                    return new NullReferenceException("Exercise was not found");

                await exerciseReadService.Get(x => x.Id == entity.ExerciseId, "none");
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
