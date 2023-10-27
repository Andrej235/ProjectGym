namespace ProjectGym.DTOs
{
    public class NoteDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; } = null!;
        public int ExerciseId { get; set; }
    }
}
