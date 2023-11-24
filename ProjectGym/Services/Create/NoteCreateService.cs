using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class NoteCreateService(ExerciseContext context, IReadService<Exercise> exerciseReadService) : ICreateService<Note>
    {
        public async Task<object> Add(Note toAdd)
        {
            if (toAdd.ExerciseId < 1)
                return default(int);

            try
            {
                await exerciseReadService.Get(x => x.Id == toAdd.ExerciseId, "none");

                await context.Notes.AddAsync(toAdd);
                await context.SaveChangesAsync();

                return toAdd.Id;
            }
            catch (NullReferenceException)
            {

                Debug.WriteLine($"Exercise with Id {toAdd.ExerciseId} was not found");
                return default(int);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while trying to add Image: {ex.Message}");
                return default(int);
            }
        }
    }
}
