using ProjectGym.Services.DatabaseSerialization;

namespace ProjectGym.Models
{
    public class EquipmentUsage
    {
        public int Id { get; set; }
        [ModelReference("Exercise")]
        public int ExerciseId { get; set; }
        [ModelReference("Equipment")]
        public int EquipmentId { get; set; }
    }
}
