using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class WeightMapper : IEntityMapper<PersonalExerciseWeight, PersonalExerciseWeightDTO>
    {
        public PersonalExerciseWeight Map(PersonalExerciseWeightDTO dto) => new()
        {
            Weight = dto.Weight,
            IsCurrent = true,
            DateOfAchieving = DateTime.Now,
            ExerciseId = dto.ExerciseId,
            UserId = dto.UserId,
        };

        public PersonalExerciseWeightDTO Map(PersonalExerciseWeight entity) => new()
        {
            Weight = entity.Weight,
            DateOfAchieving = entity.DateOfAchieving,
            IsCurrent = entity.IsCurrent,
            ExerciseId = entity.ExerciseId,
            UserId = entity.UserId,
        };
    }
}
