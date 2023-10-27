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
            ImageUrlMain = entity.ImageUrlMain,
            ImageUrlSecondary = entity.ImageUrlSecondary,
            IsFront = entity.IsFront,
            Name_en = entity.Name_en,
            PrimaryInExerciseIds = entity.PrimaryInExercises.Select(x => x.Id),
            SecondaryInExerciseIds = entity.SecondaryInExercises.Select(x => x.Id)
        };
    }
}
