using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class MuscleMapper : IEntityMapper<Muscle, MuscleDTO>
    {
        public MuscleDTO Map(Muscle entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            ImageUrl = entity.ImageUrl,
            MuscleGroupId = entity.MuscleGroupId,
            PrimaryInExercises = entity.PrimaryInExercises.Select(x => x.Id),
            SecondaryInExercises = entity.SecondaryInExercises.Select(x => x.Id),
        };

        public Muscle Map(MuscleDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            ImageUrl = dto.ImageUrl,
            MuscleGroupId = dto.MuscleGroupId,
        };
    }
}
