using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Services.Create
{
    public class EquipmentCreateService(ExerciseContext context, IReadService<Equipment> readService) : ICreateService<Equipment>
    {
        public async Task<object> Add(Equipment toAdd)
        {
            try
            {
                await readService.Get(eq => eq.Name.ToLower() == toAdd.Name.ToLower(), "none");
                throw new Exception("Entity already exists");
            }
            catch (NullReferenceException)
            {
                try
                {
                    Equipment equipmentAddedToDB = new() { Name = toAdd.Name };
                    await context.Equipment.AddAsync(equipmentAddedToDB);
                    await context.SaveChangesAsync();

                    equipmentAddedToDB.UsedInExercises = toAdd.UsedInExercises;
                    await context.SaveChangesAsync();
                    return equipmentAddedToDB.Id;
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
