using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class WorkoutSet
    {
        [Key]
        public Guid Id { get; set; }
        public int TargetSets { get; set; }

        public Workout Workout { get; set; } = null!;
        public Guid WorkoutId { get; set; }

        public Set Set { get; set; } = null!;
        public Guid SetId { get; set; }

        public Set? Superset { get; set; }
        public Guid? SuperSetId { get; set; }
    }
}
