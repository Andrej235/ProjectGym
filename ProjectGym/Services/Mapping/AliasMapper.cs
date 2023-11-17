using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class AliasMapper : IEntityMapperSync<Alias, ExerciseAliasDTO>
    {
        public ExerciseAliasDTO Map(Alias entity) => new()
        {
            Id = entity.Id,
            Alias = entity.AliasName,
            ExerciseId = entity.ExerciseId,
        };

        public Alias Map(ExerciseAliasDTO dto) => new()
        {
            AliasName = dto.Alias,
            ExerciseId = dto.ExerciseId,
        };
    }
}
