using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class UserCreateService : ICreateService<User, Guid>
    {
        private readonly ExerciseContext context;
        private readonly IReadService<User> readService;
        public UserCreateService(ExerciseContext context, IReadService<User> readService)
        {
            this.context = context;
            this.readService = readService;
        }

        public async Task<Guid> Add(User toAdd)
        {
            try
            {
                await readService.Get(eq => eq.Email == toAdd.Email, "none");
                return default;
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
                    Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                return default;
            }
        }
    }
}
