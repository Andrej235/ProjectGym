using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class SetMapper : IEntityMapper<Set, SetDTO>
    {
        public Set Map(SetDTO dto) => new()
        {
            Id = dto.Id,
            RepRange_Top = dto.RepRange_Top,
            RepRange_Bottom = dto.RepRange_Bottom,
            ToFaliure = dto.ToFailure,
            CreatorId = dto.CreatorId,
            ExerciseId = dto.ExerciseId,
            DropSet = dto.DropSet,
        };

        public SetDTO Map(Set entity) => new()
        {
            Id = entity.Id,
            ToFailure = entity.ToFaliure,
            RepRange_Top = entity.RepRange_Top,
            RepRange_Bottom = entity.RepRange_Bottom,
            CreatorId = entity.CreatorId,
            ExerciseId = entity.ExerciseId,
            DropSet = entity.DropSet,
        };
    }
}
