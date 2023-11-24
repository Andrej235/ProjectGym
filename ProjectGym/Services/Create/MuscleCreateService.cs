using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class MuscleCreateService(ExerciseContext context) : ICreateService<Muscle>
    {
        public async Task<object> Add(Muscle toAdd)
        {
            try
            {
                await context.Muscles.AddAsync(toAdd);
                await context.SaveChangesAsync();
                return toAdd.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                return default(int);
            }
        }
    }
}