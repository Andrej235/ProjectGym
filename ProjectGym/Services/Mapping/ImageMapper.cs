using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class ImageMapper : IEntityMapperSync<Image, ImageDTO>
    {
        public ImageDTO Map(Image entity) => new()
        {
            Id = entity.Id,
            ExerciseId = entity.ExerciseId,
            ImageURL = entity.ImageURL,
        };

        public Image Map(ImageDTO dto) => new()
        {
            Id = dto.Id,
            ExerciseId = dto.ExerciseId,
            ImageURL = dto.ImageURL,
        };
    }
}
