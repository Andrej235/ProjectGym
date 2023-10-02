namespace ProjectGym.Models
{
    public class ExerciseVideo
    {
        public int Id { get; set; }
        public string UUID { get; set; } = null!;
        public int Exercise_base { get; set; }
        public string ExerciseBaseUUID { get; set; } = null!;
        public string Video { get; set; } = null!;
        public bool IsMain { get; set; }
        public int Size { get; set; }
        public string Duration { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Codec { get; set; } = null!;
        public string CodecLong { get; set; } = null!;
    }
}