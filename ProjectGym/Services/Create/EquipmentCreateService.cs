using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;
using System.Text.Json;

namespace ProjectGym.Services.Create
{
    public class EquipmentCreateService : ICreateService<Equipment>
    {
        private readonly ExerciseContext context;
        private readonly IReadService<Equipment> readService;
        public EquipmentCreateService(ExerciseContext context, IReadService<Equipment> readService)
        {
            this.context = context;
            this.readService = readService;
        }

        public async Task<bool> Add(Equipment toAdd)
        {
            try
            {
                await readService.Get(eq => eq.Name.ToLower() == toAdd.Name.ToLower(), "none");
                return false;
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
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                return false;
            }
        }
    }
}
