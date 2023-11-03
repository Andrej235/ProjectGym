using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class ClientCreateService : ICreateService<Client>
    {
        private readonly ExerciseContext context;

        public ClientCreateService(ExerciseContext context)
        {
            this.context = context;
        }

        public async Task<bool> Add(Client toAdd)
        {
            try
            {
                if (toAdd.User != null)
                    context.Attach(toAdd.User);

                await context.Clients.AddAsync(toAdd);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                return false;
            }
        }
    }
}
