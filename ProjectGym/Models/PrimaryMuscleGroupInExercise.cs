using ProjectGym.Services.DatabaseSerialization;

namespace ProjectGym.Models
{
    public class PrimaryMuscleGroupInExercise
    {
        public int Id { get; set; }
        [ModelReference("Exercise")]
        public int ExerciseId { get; set; }
        [ModelReference("MuscleGroup")]
        public int MuscleGroupId { get; set; }
    }
}