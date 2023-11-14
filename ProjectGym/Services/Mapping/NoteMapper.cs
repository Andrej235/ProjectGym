using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class NoteMapper : IEntityMapperSync<ExerciseNote, NoteDTO>
    {
        public NoteDTO Map(ExerciseNote entity) => new()
        {
            Id = entity.Id,
            Comment = entity.Comment,
            ExerciseId = entity.ExerciseId
        };

        public ExerciseNote Map(NoteDTO dto) => new()
        {
            Comment = dto.Comment,
            ExerciseId = dto.ExerciseId,
        };
    }
}
