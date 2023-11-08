namespace AppProjectGym.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public IEnumerable<int> ImageIds { get; set; } = new List<int>();
        public IEnumerable<int> VideoIds { get; set; } = new List<int>();
        public IEnumerable<int> IsVariationOfIds { get; set; } = new List<int>();
        public IEnumerable<int> VariationIds { get; set; } = new List<int>();
        public IEnumerable<int> EquipmentIds { get; set; } = new List<int>();
        public IEnumerable<int> PrimaryMuscleIds { get; set; } = new List<int>();
        public IEnumerable<int> SecondaryMuscleIds { get; set; } = new List<int>();
        public IEnumerable<int> AliasIds { get; set; } = new List<int>();
        public IEnumerable<int> NoteIds { get; set; } = new List<int>();
    }
}
