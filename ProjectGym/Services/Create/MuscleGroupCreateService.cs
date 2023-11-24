using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class MuscleGroupCreateService(ExerciseContext context, IReadService<MuscleGroup> readService) : ICreateService<MuscleGroup>
    {
        public async Task<object> Add(MuscleGroup toAdd)
        {
            try
            {
                await readService.Get(x => x.Name.ToLower().Trim() == toAdd.Name.ToLower().Trim(), "none");
                throw new Exception("Entity already exists");
            }
            catch (NullReferenceException)
            {
                try
                {
                    await context.MuscleGroups.AddAsync(toAdd);
                    await context.SaveChangesAsync();
                    return toAdd.Id;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
