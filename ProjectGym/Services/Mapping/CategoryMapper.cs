using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class CategoryMapper : IEntityMapper<ExerciseCategory, CategoryDTO>
    {
        public CategoryDTO MapEntity(ExerciseCategory entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            ExerciseIds = entity.Exercises.Select(e => e.Id)
        };
    }
}
