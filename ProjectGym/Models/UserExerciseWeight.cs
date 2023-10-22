﻿using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class UserExerciseWeight
    {
        [Key]
        public Guid Id { get; set; }
        public int Weight { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime? DateOfAchieving { get; set; }

        public Exercise Exercise { get; set; } = null!;
        public int ExerciseId { get; set; }

        public User User { get; set; } = null!;
        public Guid UserId { get; set; } 
    }
}
