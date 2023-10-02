namespace ProjectGym.Models
{
    public class Muscle
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Name_en { get; set; } = null!;
        public bool IsFront { get; set; }
        public string ImageUrlMain { get; set; } = null!;
        public string ImageUrlSecondary { get; set; } = null!;
    }
}