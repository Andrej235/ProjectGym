using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class AliasMapper : IEntityMapper<ExerciseAlias, ExerciseAliasDTO>
    {
        public ExerciseAliasDTO Map(ExerciseAlias entity) => new()
        {
            Id = entity.Id,
            Alias = entity.Alias,
            ExerciseId = entity.ExerciseId,
        };

        public ExerciseAlias Map(ExerciseAliasDTO dto) => new()
        {
            Alias = dto.Alias,
            ExerciseId = dto.ExerciseId,
        };
    }
}
