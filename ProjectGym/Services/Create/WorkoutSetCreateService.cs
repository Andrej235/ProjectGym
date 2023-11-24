using ProjectGym.Data;
using ProjectGym.Models;

namespace ProjectGym.Services.Create
{
    public class WorkoutSetCreateService(ExerciseContext context) : ICreateService<WorkoutSet>
    {
        public async Task<object> Add(WorkoutSet toAdd)
        {
            await context.AddAsync(toAdd);
            await context.SaveChangesAsync();
            return toAdd.Id;
        }
    }
}
