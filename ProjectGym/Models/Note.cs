namespace ProjectGym.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string NoteText { get; set; } = null!;

        public Exercise Exercise { get; set; } = null!;
        public int ExerciseId { get; set; }
    }
}