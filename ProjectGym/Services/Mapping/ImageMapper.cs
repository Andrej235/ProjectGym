using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class ImageMapper : IEntityMapper<ExerciseImage, ImageDTO>
    {
        public ImageDTO Map(ExerciseImage entity) => new()
        {
            Id = entity.Id,
            ImageURL = entity.ImageURL,
            IsMain = entity.IsMain,
            Style = entity.Style,
        };
    }
}
