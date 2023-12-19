using AppProjectGym.Models;
using AppProjectGym.Services.Read;
using AppProjectGym.Utilities;
using System.Diagnostics;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Services
{
    public class ExerciseDisplayMapper(IReadService readService)
    {
        /// <summary>
        /// Returns an instance of exercise mapper containing the given exercise an the first image which HAS TO BE included in it
        /// <br/>If given exercise doesn't contain any image ids the result will have a new instance of Image asigned to it
        /// </summary>
        public async Task<ExerciseDisplay> Map(Exercise exercise)
        {
            Image image = new();
            try
            {
                var imageId = exercise.ImageIds.FirstOrDefault();
                if (imageId != 0)
                    image = (await readService.Get<Image>("none", $"image/{imageId}"));
                /*else
                {
                    var images = await readService.Get<List<Image>>("none", ReadService.TranslateEndPoint("image", 0, 1), $"exercise={exercise.Id}");
                    if (images != null && images.Count != 0)
                        image = images.First();
                }*/
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
            }

            return new()
            {
                Exercise = exercise,
                Image = image
            };
        }
    }
}
