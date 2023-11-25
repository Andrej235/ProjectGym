using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
{
    public class UserCreateService(ExerciseContext context, IReadService<User> readService) : CreateService<User>(context)
    {
        protected override async Task<Exception?> IsEntityValid(User entity)
        {
            try
            {
                await readService.Get(eq => eq.Email == entity.Email, "none");
                throw new Exception("Entity already exists");
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
