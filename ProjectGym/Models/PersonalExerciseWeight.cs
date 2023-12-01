using ProjectGym.Services.DatabaseSerialization;
using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class PersonalExerciseWeight
    {
        [Key]
        public Guid Id { get; set; }
        public float Weight { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime DateOfAchieving { get; set; }

        public Exercise Exercise { get; set; } = null!;
        [ModelReference("Exercise")]
        public int ExerciseId { get; set; }

        public User User { get; set; } = null!;
        [ModelReference("User")]
        public Guid UserId { get; set; }
    }
}
