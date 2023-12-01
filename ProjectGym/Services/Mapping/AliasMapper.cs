using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class AliasMapper : IEntityMapper<Alias, AliasDTO>
    {
        public AliasDTO Map(Alias entity) => new()
        {
            Id = entity.Id,
            Alias = entity.AliasName,
            ExerciseId = entity.ExerciseId,
        };

        public Alias Map(AliasDTO dto) => new()
        {
            Id = dto.Id,
            AliasName = dto.Alias,
            ExerciseId = dto.ExerciseId,
        };
    }
}
