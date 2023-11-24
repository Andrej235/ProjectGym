using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class NoteCreateService(ExerciseContext context, IReadService<Exercise> exerciseReadService) : ICreateService<Note>
    {
        public async Task<object> Add(Note toAdd)
        {
            if (toAdd.ExerciseId < 1)
                throw new NullReferenceException($"Exercise with Id {toAdd.ExerciseId} was not found");

            try
            {
                await exerciseReadService.Get(x => x.Id == toAdd.ExerciseId, "none");

                await context.Notes.AddAsync(toAdd);
                await context.SaveChangesAsync();

                return toAdd.Id;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                throw;
            }
        }
    }
}
