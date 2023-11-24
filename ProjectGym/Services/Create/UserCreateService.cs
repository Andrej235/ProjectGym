using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Services.Create
{
    public class UserCreateService(ExerciseContext context, IReadService<User> readService) : ICreateService<User>
    {
        public async Task<object> Add(User toAdd)
        {
            try
            {
                await readService.Get(eq => eq.Email == toAdd.Email, "none");
                throw new Exception("Entity already exists");
            }
            catch (NullReferenceException)
            {
                try
                {
                    await context.AddAsync(toAdd);
                    await context.SaveChangesAsync();
                    return toAdd.Id;
                }
                catch (Exception ex)
                {
                    LogDebugger.LogError(ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                throw;
            }
        }
    }
}
