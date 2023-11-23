using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class WorkoutMapper : IEntityMapperSync<Workout, WorkoutDTO>
    {
        public Workout Map(WorkoutDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            CreatorId = dto.CreatorId,
            IsPublic = dto.IsPublic,
        };

        public WorkoutDTO Map(Workout entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsPublic = entity.IsPublic,
            CreatorId = entity.CreatorId,
            WorkoutSetIds = entity.WorkoutSets.Select(x => x.Id),
        };
    }
}
