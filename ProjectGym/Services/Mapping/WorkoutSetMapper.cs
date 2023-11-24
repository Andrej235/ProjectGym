using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class WorkoutSetMapper : IEntityMapper<WorkoutSet, WorkoutSetDTO>
    {
        public WorkoutSet Map(WorkoutSetDTO dto) => new()
        {
            Id = dto.Id,
            TargetSets = dto.TargetSets,
            SetId = dto.SetId,
            SuperSetId = dto.SuperSetId,
            WorkoutId = dto.WorkoutId,
        };

        public WorkoutSetDTO Map(WorkoutSet entity) => new()
        {
            Id = entity.Id,
            TargetSets = entity.TargetSets,
            SetId = entity.SetId,
            SuperSetId = entity.SuperSetId,
            WorkoutId = entity.WorkoutId,
        };
    }
}
