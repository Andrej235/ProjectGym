using ProjectGym.Data;
using ProjectGym.Models;

namespace ProjectGym.Services.Create
{
    public class WorkoutCreateService(ExerciseContext context) : ICreateService<Workout, Guid>
    {
        public async Task<Guid> Add(Workout toAdd)
        {
            await context.AddAsync(toAdd);
            await context.SaveChangesAsync();
            return toAdd.Id;
        }
    }
}
