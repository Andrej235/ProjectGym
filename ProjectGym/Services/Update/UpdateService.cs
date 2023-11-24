using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;

namespace ProjectGym.Services.Update
{
    public class UpdateService<T>(ExerciseContext context) : IUpdateService<T> where T : class
    {
        public async Task Update(T updatedEntity)
        {
            try
            {
                if (!await context.Set<T>().ContainsAsync(updatedEntity))
                    throw new NullReferenceException("Entity not found");

                context.Set<T>().Update(updatedEntity);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
