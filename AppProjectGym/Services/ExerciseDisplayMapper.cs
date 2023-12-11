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
            var imgUrl = "";
            try
            {
                var id = exercise.ImageIds.FirstOrDefault();
                if (id != 0)
                    imgUrl = (await readService.Get<Image>("none", $"image/{id}")).ImageURL;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message}");
            }

            return new()
            {
                Id = exercise.Id,
                Name = exercise.Name,
                ImageUrl = imgUrl
            };
        }
    }
}
