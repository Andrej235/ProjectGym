using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;

namespace ProjectGym.Services.Create
{
    public class EquipmentCreateService : ICreateService<Equipment>
    {
        private readonly ExerciseContext context;
        public EquipmentCreateService(ExerciseContext context)
        {
            this.context = context;
        }

        public async Task<bool> Add(Equipment toAdd)
        {
            try
            {
                var equipmentInDB = await context.Equipment.FirstOrDefaultAsync(eq => eq.Name.ToLower() == toAdd.Name.ToLower());
                if (equipmentInDB != null)
                    return false;

                await context.AddAsync(toAdd);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
