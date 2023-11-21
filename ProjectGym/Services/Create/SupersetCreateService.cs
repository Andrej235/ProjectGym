using ProjectGym.Data;
using ProjectGym.Models;

namespace ProjectGym.Services.Create
{
    public class SupersetCreateService(ExerciseContext context) : ICreateService<Superset, Guid>
    {
        public async Task<Guid> Add(Superset toAdd)
        {
            await context.AddAsync(toAdd);
            await context.SaveChangesAsync();
            return toAdd.Id;
        }
    }
}
