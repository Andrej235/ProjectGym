using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Mapping
{
    public class EquipmentMapper : IEntityMapperAsync<Equipment, EquipmentDTO>
    {
        private readonly IReadService<Exercise> exerciseReadService;
        public EquipmentMapper(IReadService<Exercise> exerciseReadService)
        {
            this.exerciseReadService = exerciseReadService;
        }

        public EquipmentDTO Map(Equipment entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            UsedInExerciseIds = entity.UsedInExercises.Select(e => e.Id)
        };

        public async Task<Equipment> Map(EquipmentDTO dto)
        {
            List<Exercise> usedInExercises = new();
            foreach (var exercise in dto.UsedInExerciseIds)
            {
                try
                {
                    usedInExercises.Add(await exerciseReadService.Get(e => e.Id == exercise, "none"));
                }
                catch (Exception)
                {
                    Debug.WriteLine($"---> Exercise not found while trying to map equipment to exercise with id {exercise}");
                }
            }

            return new()
            {
                Name = dto.Name,
                UsedInExercises = usedInExercises
            };
        }
    }
}
