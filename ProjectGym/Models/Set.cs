using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class Set
    {
        [Key]
        public Guid Id { get; set; }
        public int RepRange_Bottom { get; set; }
        public int RepRange_Top { get; set; }
        public bool Partials { get; set; }
        public bool ToFaliure { get; set; }

        public Exercise Exercise { get; set; } = null!;
        public int ExerciseId { get; set; }

        public User Creator { get; set; } = null!;
        public Guid CreatorId { get; set; }
    }
}
