using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;
using System.Linq.Expressions;

namespace ProjectGym.Services.Delete
{
    public class DeleteService<T>(ExerciseContext context, IReadService<T> readService) : IDeleteService<T> where T : class
    {
        public async Task DeleteFirst(Expression<Func<T, bool>> criteria)
        {
            try
            {
                T entityToDelete = await readService.Get(criteria, "none");

                context.Set<T>().Remove(entityToDelete);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> An error occurred while trying to delete entity: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAll(Expression<Func<T, bool>> criteria)
        {
            List<T> entitiesToDelete = await readService.Get(criteria, 0, -1, "none");

            if (entitiesToDelete.Count != 0)
            {
                try
                {
                    context.Set<T>().RemoveRange(entitiesToDelete);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"---> An error occurred while trying to delete entities: {ex.Message}");
                    return false;
                }
            }
            return false;
        }
    }
}
