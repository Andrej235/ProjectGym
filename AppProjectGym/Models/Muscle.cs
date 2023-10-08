﻿namespace AppProjectGym.Models
{
    public class Muscle
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Name_en { get; set; } = null!;
        public bool IsFront { get; set; }
        public string ImageUrlMain { get; set; } = null!;
        public string ImageUrlSecondary { get; set; } = null!;

        public IEnumerable<int> PrimaryInExerciseIds { get; set; }
        public IEnumerable<int> SecondaryInExerciseIds { get; set; }
    }
}