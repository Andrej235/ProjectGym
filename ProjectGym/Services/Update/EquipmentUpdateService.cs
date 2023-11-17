using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Update
{
    public class EquipmentUpdateService : IUpdateService<Equipment>
    {
        private readonly ExerciseContext context;
        private readonly IReadService<Equipment> readService;
        private readonly IDeleteService<EquipmentUsage> equipmentExerciseUsageDeleteService;
        private readonly ICreateService<EquipmentUsage, int> equipmentExerciseUsageCreateService;
        public EquipmentUpdateService(ExerciseContext context, IReadService<Equipment> readService, IDeleteService<EquipmentUsage> equipmentExerciseUsageDeleteService, ICreateService<EquipmentUsage, int> equipmentExerciseUsageCreateService)
        {
            this.context = context;
            this.readService = readService;
            this.equipmentExerciseUsageDeleteService = equipmentExerciseUsageDeleteService;
            this.equipmentExerciseUsageCreateService = equipmentExerciseUsageCreateService;
        }

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

            //context.RemoveRange(context.EquipmentExerciseUsages.Where(eeu => eeu.EquipmentId == entity.Id));
            //context.AddRange(updatedEntity.UsedInExercises.Select(e => new EquipmentExerciseUsage() { EquipmentId = entity.Id, ExerciseId = e.Id }));

            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
