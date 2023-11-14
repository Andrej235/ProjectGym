using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Mapping
{
    public class CategoryMapper : IEntityMapperAsync<ExerciseCategory, CategoryDTO>
    {
        private readonly IReadService<Exercise> exerciseReadService;
        public CategoryMapper(IReadService<Exercise> exerciseReadService)
        {
            this.exerciseReadService = exerciseReadService;
        }

        public CategoryDTO Map(ExerciseCategory entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            ExerciseIds = entity.Exercises.Select(e => e.Id)
        };

        public async Task<ExerciseCategory> Map(CategoryDTO dto)
        {
            List<Exercise> exercises = new();
            foreach (var exercise in dto.ExerciseIds)
            {
                try
                {
                    exercises.Add(await exerciseReadService.Get(e => e.Id == exercise, "none"));
                }
                catch (Exception)
                {
                    Debug.WriteLine($"---> Exercise not found while trying to map category. Exercise id {exercise}");
                }
            }

            return new()
            {
                Name = dto.Name,
                Exercises = exercises
            };
        }
    }
}
