using AppProjectGym.Models;
using AppProjectGym.Services.Read;
using AppProjectGym.Utilities;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Services.Mapping
{
    public class ExerciseDisplayMapper(IReadService readService) : IEntityDisplayMapper<Exercise, ExerciseDisplay>
    {
        public async Task<ExerciseDisplay> Map(Exercise entity)
        {
            Image image = new();
            try
            {
                var imageId = entity.ImageIds.FirstOrDefault();
                if (imageId != 0)
                    image = (await readService.Get<Image>("none", $"image/{imageId}"));
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
            }

            return new()
            {
                Exercise = entity,
                Image = image
            };
        }
    }
}
