using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class EquipmentMapper : IEntityMapper<Equipment, EquipmentDTO>
    {
        public EquipmentDTO MapEntity(Equipment entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            UsedInExerciseIds = entity.UsedInExercises.Select(e => e.Id)
        };
    }
}
