using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Mapping
{
    public class EquipmentMapper : IEntityMapperSync<Equipment, EquipmentDTO>
    {
        public EquipmentDTO Map(Equipment entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            UsedInExerciseIds = entity.UsedInExercises.Select(e => e.Id)
        };

        public Equipment Map(EquipmentDTO dto) => new()
        {
            Name = dto.Name,
        };
    }
}
