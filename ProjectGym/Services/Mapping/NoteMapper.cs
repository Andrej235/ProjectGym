using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class NoteMapper : IEntityMapperSync<Note, NoteDTO>
    {
        public NoteDTO Map(Note entity) => new()
        {
            Id = entity.Id,
            Note = entity.NoteText,
            ExerciseId = entity.ExerciseId
        };

        public Note Map(NoteDTO dto) => new()
        {
            Id = dto.Id,
            NoteText = dto.Note,
            ExerciseId = dto.ExerciseId,
        };
    }
}
