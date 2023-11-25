using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
{
    public class NoteCreateService(ExerciseContext context, IReadService<Exercise> exerciseReadService) : CreateService<Note>(context)
    {
        protected override async Task<Exception?> IsEntityValid(Note entity)
        {
            try
            {
                if (entity.ExerciseId < 1)
                    return new NullReferenceException($"Exercise with Id {entity.ExerciseId} was not found");

                await exerciseReadService.Get(x => x.Id == entity.ExerciseId, "none");
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
