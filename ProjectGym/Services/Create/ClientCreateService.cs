using ProjectGym.Data;
using ProjectGym.Models;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class ClientCreateService(ExerciseContext context) : ICreateService<Client>
    {
        public async Task<object> Add(Client toAdd)
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
                return default(Guid);
            }
        }
    }
}
