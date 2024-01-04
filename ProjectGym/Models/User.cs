using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;

        public IEnumerable<PersonalExerciseWeight> Weights { get; set; } = new List<PersonalExerciseWeight>();
        public IEnumerable<Workout> CreatedWorkouts { get; set; } = new List<Workout>();
        public IEnumerable<Set> CreatedExerciseSets { get; set; } = new List<Set>();

        public IEnumerable<Exercise> Bookmarks { get; set; } = new List<Exercise>();
    }
}
