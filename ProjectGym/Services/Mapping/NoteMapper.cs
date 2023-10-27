using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class NoteMapper : IEntityMapper<ExerciseNote, NoteDTO>
    {
        public NoteDTO MapEntity(ExerciseNote entity) => new()
        {
            Id = entity.Id,
            Comment = entity.Comment,
            ExerciseId = entity.ExerciseId
        };
    }
}
