﻿namespace AppProjectGym.Models
{
    public class Workout
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPublic { get; set; }
        public Guid CreatorId { get; set; }
        public IEnumerable<Guid> WorkoutSetIds { get; set; } = new List<Guid>();
    }
}
