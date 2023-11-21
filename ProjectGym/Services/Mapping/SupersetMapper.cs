using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class SupersetMapper : IEntityMapperSync<Superset, SupersetDTO>
    {
        public Superset Map(SupersetDTO dto) => new()
        {
            Id = dto.Id,
            TargetSets = dto.TargetSets,
            DropSets = dto.DropSets,
            SetId = dto.SetId,
        };

        public SupersetDTO Map(Superset entity) => new()
        {
            Id = entity.Id,
            DropSets = entity.DropSets,
            TargetSets = entity.TargetSets,
            SetId = entity.SetId
        };
    }
}
