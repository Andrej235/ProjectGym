using ProjectGym.Services;

namespace ProjectGym.DTOs
{
    public class ExerciseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public int CategoryId { get; set; }
        public IEnumerable<ImageDTO> Images { get; set; } = null!;
        public IEnumerable<int> VideoIds { get; set; } = null!;
        public IEnumerable<int> IsVariationOfIds { get; set; } = null!;
        public IEnumerable<int> VariationIds { get; set; } = null!;
        public IEnumerable<int> EquipmentIds { get; set; } = null!;
        public IEnumerable<int> PrimaryMuscleIds { get; set; } = null!;
        public IEnumerable<int> SecondaryMuscleIds { get; set; } = null!;
        public IEnumerable<int> AliasIds { get; set; } = null!;
        public IEnumerable<int> NoteIds { get; set; } = null!;
    }
}
