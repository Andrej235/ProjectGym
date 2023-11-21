using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class Workout
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPublic { get; set; }

        public User Creator { get; set; } = null!;
        public Guid CreatorId { get; set; }

        public IEnumerable<WorkoutSet> WorkoutSets { get; set; } = new List<WorkoutSet>();
    }
}
