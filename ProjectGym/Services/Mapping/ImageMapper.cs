using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class ImageMapper : IEntityMapperSync<ExerciseImage, ImageDTO>
    {
        public ImageDTO Map(ExerciseImage entity) => new()
        {
            Id = entity.Id,
            ExerciseId = entity.ExerciseId,
            ImageURL = entity.ImageURL,
            IsMain = entity.IsMain,
            Style = entity.Style,
        };

        public ExerciseImage Map(ImageDTO dto) => new()
        {
            ExerciseId = dto.ExerciseId,
            ImageURL = dto.ImageURL,
            IsMain = dto.IsMain,
            Style = dto.Style,
            UUID = ""
        };
    }
}
