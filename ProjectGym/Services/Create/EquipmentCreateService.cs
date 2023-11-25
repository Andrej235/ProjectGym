using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
{
    public class EquipmentCreateService(ExerciseContext context, IReadService<Equipment> readService) : CreateService<Equipment>(context)
    {
        protected override async Task<Exception?> IsEntityValid(Equipment entity)
        {
            try
            {
                await readService.Get(eq => eq.Name.ToLower() == entity.Name.ToLower(), "none");
                return new Exception("Entity already exists");
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
