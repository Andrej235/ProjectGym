using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Update
{
    public class ClientUpdateService(ExerciseContext context, IReadService<Client> readService) : IUpdateService<Client>
    {
        public async Task Update(Client updatedEntity)
        {
            var entity = await readService.Get(x => x.Id == updatedEntity.Id, "user");
            context.Attach(entity);

            if (updatedEntity.User is not null)
                context.Attach(updatedEntity.User);

            if (entity.User is not null)
                context.Attach(entity.User);

            entity.User = updatedEntity.User;
            await context.SaveChangesAsync();
        }
    }
}
