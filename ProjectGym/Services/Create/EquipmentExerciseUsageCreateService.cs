using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class EquipmentExerciseUsageCreateService(ExerciseContext context, IReadService<EquipmentUsage> readService) : ICreateService<EquipmentUsage, int>
    {
        public async Task<int> Add(EquipmentUsage toAdd)
        {
            try
            {
                await readService.Get(x => x.EquipmentId == toAdd.EquipmentId && x.ExerciseId == toAdd.ExerciseId, "none");
                return default;
            }
            catch (NullReferenceException)
            {
                try
                {
                    await context.EquipmentUsages.AddAsync(toAdd);
                    await context.SaveChangesAsync();
                    return toAdd.Id;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error occured while trying to add EquipmentExerciseUsage: {ex.Message}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while trying to add EquipmentExerciseUsage: {ex.Message}");
                return default;
            }
        }
    }
}
