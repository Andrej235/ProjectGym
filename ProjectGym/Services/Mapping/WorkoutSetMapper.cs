using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class WorkoutSetMapper : IEntityMapperSync<WorkoutSet, WorkoutSetDTO>
    {
        public WorkoutSet Map(WorkoutSetDTO dto) => new()
        {
            Id = dto.Id,
            DropSets = dto.DropSets,
            TargetSets = dto.TargetSets,
            SetId = dto.SetId,
            SuperSetId = dto.SuperSetId,
            WorkoutId = dto.WorkoutId,
        };

        public WorkoutSetDTO Map(WorkoutSet entity) => new()
        {
            Id = entity.Id,
            DropSets = entity.DropSets,
            TargetSets = entity.TargetSets,
            SetId = entity.SetId,
            SuperSetId = entity.SuperSetId,
            WorkoutId = entity.WorkoutId,
        };
    }
}
