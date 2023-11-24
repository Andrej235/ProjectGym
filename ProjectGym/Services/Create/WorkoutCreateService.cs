using ProjectGym.Data;
using ProjectGym.Models;

namespace ProjectGym.Services.Create
{
    public class WorkoutCreateService(ExerciseContext context) : ICreateService<Workout>
    {
        public async Task<object> Add(Workout toAdd)
        {
            await context.AddAsync(toAdd);
            await context.SaveChangesAsync();
            return toAdd.Id;
        }
    }
}
