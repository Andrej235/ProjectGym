using ProjectGym.Services.DatabaseSerialization;

namespace ProjectGym.Models
{
    public class SecondaryMuscleGroupInExercise
    {
        public int Id { get; set; }
        [ModelReference("Exercise")]
        public int ExerciseId { get; set; }
        [ModelReference("Muscle")]
        public int MuscleGroupId { get; set; }
    }
}
