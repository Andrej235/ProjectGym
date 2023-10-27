using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Utilities;

namespace ProjectGym.Services.Mapping
{
    public class EquipmentMapper : IEntityMapperAsync<Equipment, EquipmentDTO>
    {
        private readonly ExerciseContext context;
        public EquipmentMapper(ExerciseContext context)
        {
            this.context = context;
        }

        public EquipmentDTO Map(Equipment entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            UsedInExerciseIds = entity.UsedInExercises.Select(e => e.Id)
        };

        public async Task<Equipment> Map(EquipmentDTO dto) => new()
        {
            Name = dto.Name,
            UsedInExercises = (await Task.WhenAll(
                dto.UsedInExerciseIds.Select(
                    async x => await context.Exercises.FindAsync(x))
                )).SelectNotNull(),
        };
    }
}
