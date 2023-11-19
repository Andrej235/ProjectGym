using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class ClientCreateService(ExerciseContext context) : ICreateService<Client, Guid>
    {
        public async Task<Guid> Add(Client toAdd)
        {
            try
            {
                if (toAdd.User != null)
                    context.Attach(toAdd.User);

                await context.Clients.AddAsync(toAdd);
                await context.SaveChangesAsync();
                return toAdd.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                return default;
            }
        }
    }
}
