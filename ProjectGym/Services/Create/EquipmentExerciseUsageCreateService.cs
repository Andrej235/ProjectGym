using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
{
    public class EquipmentExerciseUsageCreateService(ExerciseContext context, IReadService<EquipmentUsage> readService) : CreateService<EquipmentUsage>(context)
    {
        protected override async Task<Exception?> IsEntityValid(EquipmentUsage entity)
        {
            try
            {
                await readService.Get(x => x.EquipmentId == entity.EquipmentId && x.ExerciseId == entity.ExerciseId, "none");
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
