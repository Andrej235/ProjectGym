using ProjectGym.Services;

namespace ProjectGym.DTOs
{
    public class ExerciseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public IEnumerable<int> ImageIds { get; set; } = new List<int>();
        public IEnumerable<int> EquipmentIds { get; set; } = new List<int>();
        public IEnumerable<int> PrimaryMuscleGroupIds { get; set; } = new List<int>();
        public IEnumerable<int> SecondaryMuscleGroupIds { get; set; } = new List<int>();
        public IEnumerable<int> PrimaryMuscleIds { get; set; } = new List<int>();
        public IEnumerable<int> SecondaryMuscleIds { get; set; } = new List<int>();
        public IEnumerable<int> AliasIds { get; set; } = new List<int>();
        public IEnumerable<int> NoteIds { get; set; } = new List<int>();
        public IEnumerable<int> CommentIds { get; set; } = new List<int>();
    }
}
