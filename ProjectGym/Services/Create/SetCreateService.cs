using ProjectGym.Data;
using ProjectGym.Models;

namespace ProjectGym.Services.Create
{
    public class SetCreateService(ExerciseContext context) : ICreateService<Set>
    {
        public async Task<object> Add(Set toAdd)
        {
            await context.AddAsync(toAdd);
            await context.SaveChangesAsync();
            return toAdd.Id;
        }
    }
}
