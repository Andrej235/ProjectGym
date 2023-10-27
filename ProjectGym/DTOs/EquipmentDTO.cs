namespace ProjectGym.DTOs
{
    public class EquipmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<int> UsedInExerciseIds { get; set; } = new List<int>();
    }
}
