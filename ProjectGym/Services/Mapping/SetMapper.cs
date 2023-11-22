using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class SetMapper : IEntityMapperSync<Set, SetDTO>
    {
        public Set Map(SetDTO dto) => new()
        {
            Id = dto.Id,
            RepRange_Top = dto.RepRange_Top,
            RepRange_Bottom = dto.RepRange_Bottom,
            ToFaliure = dto.ToFaliure,
            CreatorId = dto.CreatorId,
            ExerciseId = dto.ExerciseId,
            DropSet = dto.DtopSet,
        };

        public SetDTO Map(Set entity) => new()
        {
            Id = entity.Id,
            ToFaliure = entity.ToFaliure,
            RepRange_Top = entity.RepRange_Top,
            RepRange_Bottom = entity.RepRange_Bottom,
            CreatorId = entity.CreatorId,
            ExerciseId = entity.ExerciseId,
            DtopSet = entity.DropSet,
        };
    }
}
