namespace ProjectGym.DTOs
{
    public class SetDTO
    {
        public Guid Id { get; set; }
        public int RepRange_Bottom { get; set; } = 8;
        public int RepRange_Top { get; set; } = 12;
        public bool Partials { get; set; } = true;
        public bool ToFaliure { get; set; } = true;

        public int ExerciseId { get; set; }
        public Guid CreatorId { get; set; }
    }
}
