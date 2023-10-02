namespace ProjectGym.Models
{
    public class ExerciseVideo
    {
        public int Id { get; set; }
        public string UUID { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
        public bool IsMain { get; set; }
        public int Size { get; set; }
        public string Duration { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Codec { get; set; } = null!;
        public string CodecLong { get; set; } = null!;

        public Exercise Exercise { get; set; } = null!;
        public int ExerciseId { get; set; }
    }
}