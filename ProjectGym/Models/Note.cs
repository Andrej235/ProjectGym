using ProjectGym.Services.DatabaseSerialization;

namespace ProjectGym.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string NoteText { get; set; } = null!;

        public Exercise Exercise { get; set; } = null!;
        [ModelReference("Exercise")]
        public int ExerciseId { get; set; }
    }
}