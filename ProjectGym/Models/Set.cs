using ProjectGym.Services.DatabaseSerialization;
using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class Set
    {
        [Key]
        public Guid Id { get; set; }
        public int RepRange_Bottom { get; set; }
        public int RepRange_Top { get; set; }
        public bool ToFaliure { get; set; }
        public bool DropSet { get; set; }

        public Exercise Exercise { get; set; } = null!;
        [ModelReference("Exercise")]
        public int ExerciseId { get; set; }

        public User Creator { get; set; } = null!;
        [ModelReference("User")]
        public Guid CreatorId { get; set; }
    }
}
