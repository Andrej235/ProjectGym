using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Mapping
{
    public class MuscleGroupMapper : IEntityMapperSync<MuscleGroup, MuscleGroupDTO>
    {
        public MuscleGroupDTO Map(MuscleGroup entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            MuscleIds = entity.Muscles.Select(x => x.Id),
            PrimaryInExercises = entity.PrimaryInExercises.Select(x => x.Id),
            SecondaryInExercises = entity.SecondaryInExercises.Select(x => x.Id),
        };

        public MuscleGroup Map(MuscleGroupDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
        };
    }
}
