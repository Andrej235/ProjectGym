using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class AliasMapper : IEntityMapper<ExerciseAlias, ExerciseAliasDTO>
    {
        public ExerciseAliasDTO MapEntity(ExerciseAlias entity) => new()
        {
            Id = entity.Id,
            Alias = entity.Alias,
            ExerciseId = entity.ExerciseId,
        };
    }
}
