using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Update
{
    public class EquipmentUpdateService(ExerciseContext context, IReadService<Equipment> readService) : IUpdateService<Equipment>
    {
        public async Task Update(Equipment updatedEntity)
        {
            var entity = await readService.Get(eq => eq.Id == updatedEntity.Id, "none") ?? throw new NullReferenceException("Entity not found");
            context.Attach(entity);
            entity.Name = updatedEntity.Name;
            await context.SaveChangesAsync();
        }
    }
}
