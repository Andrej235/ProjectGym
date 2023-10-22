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

        public IEnumerable<UserExerciseWeight> Weights { get; set; } = new List<UserExerciseWeight>();
        public IEnumerable<Workout> CreatedWorkouts { get; set; } = new List<Workout>();
        public IEnumerable<Set> CreatedExerciseSets { get; set; } = new List<Set>();

        public IEnumerable<ExerciseComment> ExerciseComments { get; set; } = new List<ExerciseComment>();
        public IEnumerable<ExerciseComment> ExerciseCommentUpvotes { get; set; } = new List<ExerciseComment>();
        public IEnumerable<ExerciseComment> ExerciseCommentDownvotes { get; set; } = new List<ExerciseComment>();
        public IEnumerable<Exercise> ExerciseBookmarks { get; set; } = new List<Exercise>();
    }
}
