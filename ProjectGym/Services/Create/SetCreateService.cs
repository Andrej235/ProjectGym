using ProjectGym.Data;
using ProjectGym.Models;

namespace ProjectGym.Services.Create
{
    public class SetCreateService(ExerciseContext context) : ICreateService<Set, Guid>
    {
        public async Task<Guid> Add(Set toAdd)
        {
            await context.AddAsync(toAdd);
            await context.SaveChangesAsync();
            return toAdd.Id;
        }
    }
}
