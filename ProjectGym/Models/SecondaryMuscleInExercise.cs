﻿using ProjectGym.Services.DatabaseSerialization;
namespace ProjectGym.Models
{
    public class SecondaryMuscleInExercise
    {
        public int Id { get; set; }
        [ModelReference("Exercise")]
        public int ExerciseId { get; set; }
        [ModelReference("Muscle")]
        public int MuscleId { get; set; }
    }
}
