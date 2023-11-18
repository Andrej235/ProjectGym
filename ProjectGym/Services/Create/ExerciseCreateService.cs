using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class ExerciseCreateService(ExerciseContext context, IReadService<Exercise> readService) : ICreateService<Exercise, int>
    {
        public async Task<int> Add(Exercise toAdd)
        {
            try
            {
                await readService.Get(x => x.Name.ToLower().Equals(toAdd.Name.ToLower()), "none");
                return default;
            }
            catch (NullReferenceException)
            {
                try
                {
                    context.AttachRange(toAdd.Equipment);
                    context.AttachRange(toAdd.PrimaryMuscleGroups);
                    context.AttachRange(toAdd.SecondaryMuscleGroups);
                    context.AttachRange(toAdd.PrimaryMuscles);
                    context.AttachRange(toAdd.SecondaryMuscles);

                    Exercise dbEntity = new()
                    {
                        Name = toAdd.Name,
                        Description = toAdd.Description,
                        Equipment = toAdd.Equipment,
                        PrimaryMuscleGroups = toAdd.PrimaryMuscleGroups,
                        SecondaryMuscleGroups = toAdd.SecondaryMuscleGroups,
                        PrimaryMuscles = toAdd.PrimaryMuscles,
                        SecondaryMuscles = toAdd.SecondaryMuscles,
                    };

                    await context.Exercises.AddAsync(toAdd);
                    await context.SaveChangesAsync();
                    return dbEntity.Id;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                return default;
            }
        }
    }
}
