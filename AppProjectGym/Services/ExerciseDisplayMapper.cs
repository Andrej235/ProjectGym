using AppProjectGym.Models;
using AppProjectGym.Services.Read;
using System.Diagnostics;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Services
{
    public class ExerciseDisplayMapper(IReadService readService)
    {
        public async Task<ExerciseDisplay> Map(Exercise exercise)
        {
            Image image = new();
            try
            {
                var id = exercise.ImageIds.FirstOrDefault();
                if (id != 0)
                    image = (await readService.Get<Image>("none", $"image/{id}"));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message}");
            }

            return new()
            {
                Exercise = exercise,
                Image = image
            };
        }
    }
}
