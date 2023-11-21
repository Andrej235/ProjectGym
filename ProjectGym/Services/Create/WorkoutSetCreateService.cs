using ProjectGym.Data;
using ProjectGym.Models;

namespace ProjectGym.Services.Create
{
    public class WorkoutSetCreateService(ExerciseContext context) : ICreateService<WorkoutSet, Guid>
    {
        public async Task<Guid> Add(WorkoutSet toAdd)
        {
            await context.AddAsync(toAdd);
            await context.SaveChangesAsync();
            return toAdd.Id;
        }
    }
}
