using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Mapping
{
    public class MuscleMapper : IEntityMapperAsync<Muscle, MuscleDTO>
    {
        private readonly IReadService<Exercise> exerciseReadService;
        public MuscleMapper(IReadService<Exercise> exerciseReadService)
        {
            this.exerciseReadService = exerciseReadService;
        }

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

        public async Task<Muscle> Map(MuscleDTO dto)
        {
            List<Exercise> primaryInExercises = new();
            List<Exercise> secondaryInExercises = new();
            foreach (var exercise in dto.PrimaryInExerciseIds)
            {
                try
                {
                    primaryInExercises.Add(await exerciseReadService.Get(e => e.Id == exercise, "none"));
                }
                catch (Exception)
                {
                    Debug.WriteLine($"---> Exercise not found while trying to map muscle. Exercise id {exercise}");
                }
            }

            foreach (var exercise in dto.SecondaryInExerciseIds)
            {
                try
                {
                    secondaryInExercises.Add(await exerciseReadService.Get(e => e.Id == exercise, "none"));
                }
                catch (Exception)
                {
                    Debug.WriteLine($"---> Exercise not found while trying to map muscle. Exercise id {exercise}");
                }
            }

            return new()
            {
                Name = dto.Name,
                Name_en = dto.Name_en,
                ImageUrlMain = dto.ImageUrlMain,
                ImageUrlSecondary = dto.ImageUrlSecondary,
                IsFront = dto.IsFront,
                PrimaryInExercises = primaryInExercises,
                SecondaryInExercises = secondaryInExercises,
            };
        }
    }
}
