using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Update
{
    public class EquipmentUpdateService(ExerciseContext context, IReadService<Equipment> readService, IDeleteService<EquipmentUsage> equipmentExerciseUsageDeleteService, ICreateService<EquipmentUsage, int> equipmentExerciseUsageCreateService) : IUpdateService<Equipment>
    {
        public async Task Update(Equipment updatedEntity)
        {
            var entity = await readService.Get(eq => eq.Id == updatedEntity.Id, "all") ?? throw new NullReferenceException("Entity not found");

            entity.Name = updatedEntity.Name;

            await equipmentExerciseUsageDeleteService.DeleteAll(eeu => eeu.EquipmentId == entity.Id);
            var newUsages = updatedEntity.UsedInExercises.Select(e => new EquipmentUsage()
            {
                EquipmentId = entity.Id,
                ExerciseId = e.Id
            });
            foreach (var usage in newUsages)
                await equipmentExerciseUsageCreateService.Add(usage);

            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
