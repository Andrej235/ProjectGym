using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class EquipmentCreateService(ExerciseContext context, IReadService<Equipment> readService) : ICreateService<Equipment, int>
    {
        public async Task<int> Add(Equipment toAdd)
        {
            try
            {
                await readService.Get(eq => eq.Name.ToLower() == toAdd.Name.ToLower(), "none");
                return default;
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
