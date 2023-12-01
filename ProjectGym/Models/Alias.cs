using ProjectGym.Services.DatabaseSerialization;

namespace ProjectGym.Models
{
    public class Alias
    {
        public int Id { get; set; }
        public string AliasName { get; set; } = null!;

        public Exercise Exercise { get; set; } = null!;
        [ModelReference("Exercise")]
        public int ExerciseId { get; set; }
    }
}