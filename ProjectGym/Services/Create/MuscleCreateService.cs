using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class MuscleCreateService : ICreateService<Muscle, int>
    {
        private readonly ExerciseContext context;
        private readonly IReadService<Muscle> readService;
        public MuscleCreateService(ExerciseContext context, IReadService<Muscle> readService)
        {
            this.context = context;
            this.readService = readService;
        }

        public async Task<int> Add(Muscle toAdd)
        {
            try
            {
                await readService.Get(eq => eq.Name.ToLower() == toAdd.Name.ToLower(), "none");
                return default;
            }
            catch (NullReferenceException)
            {
                try
                {
                    //Muscle entityAddedToDB = new() { Name = toAdd.Name };
                    //await context.SaveChangesAsync();
                    //entityAddedToDB.PrimaryInExercises = toAdd.PrimaryInExercises;
                    //entityAddedToDB.SecondaryInExercises = toAdd.SecondaryInExercises;

                    await context.Muscles.AddAsync(toAdd);
                    await context.SaveChangesAsync();
                    return toAdd.Id;// entityAddedToDB.Id;
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