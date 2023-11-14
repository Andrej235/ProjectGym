using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class CategoryCreateService : ICreateService<ExerciseCategory, int>
    {
        private readonly ExerciseContext context;
        private readonly IReadService<ExerciseCategory> readService;

        public CategoryCreateService(ExerciseContext context, IReadService<ExerciseCategory> readService)
        {
            this.context = context;
            this.readService = readService;
        }

        public async Task<int> Add(ExerciseCategory toAdd)
        {
            try
            {
                await readService.Get(x => x.Name.ToLower() == toAdd.Name.ToLower(), "none");
                return default;
            }
            catch (NullReferenceException)
            {
                try
                {
                    ExerciseCategory entityAddedToDB = new() { Name = toAdd.Name };
                    await context.ExerciseCategories.AddAsync(entityAddedToDB);
                    await context.SaveChangesAsync();

                    entityAddedToDB.Exercises = toAdd.Exercises;
                    await context.SaveChangesAsync();
                    return entityAddedToDB.Id;
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
