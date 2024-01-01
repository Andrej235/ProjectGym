using ProjectGym.Services.DatabaseSerialization;
using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class WorkoutSet
    {
        [Key]
        public Guid Id { get; set; }
        public int TargetSets { get; set; }

        public Workout Workout { get; set; } = null!;
        [ModelReference("Workout")]
        public Guid WorkoutId { get; set; }

        public Set Set { get; set; } = null!;
        [ModelReference("Set")]
        public Guid SetId { get; set; }
    }
}
