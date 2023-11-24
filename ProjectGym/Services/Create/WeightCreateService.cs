using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;
using ProjectGym.Utilities;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class WeightCreateService(ExerciseContext context,
                                     IReadService<User> userReadService,
                                     IReadService<PersonalExerciseWeight> weightReadService,
                                     IUpdateService<PersonalExerciseWeight> weightUpdateService,
                                     IReadService<Exercise> exerciseReadService) : ICreateService<PersonalExerciseWeight>
    {
        public async Task<object> Add(PersonalExerciseWeight toAdd)
        {
            try
            {
                await userReadService.Get(x => x.Id == toAdd.UserId, "none");
                await exerciseReadService.Get(x => x.Id == toAdd.ExerciseId, "none");

                if (toAdd.IsCurrent)
                {
                    (await weightReadService.Get(x => x.UserId == toAdd.UserId && x.ExerciseId == toAdd.ExerciseId && x.IsCurrent, 0, -1, "none")).ForEach(x =>
                    {
                        x.IsCurrent = false;
                        weightUpdateService.Update(x);
                    });
                }

                await context.Weights.AddAsync(toAdd);
                await context.SaveChangesAsync();
                return toAdd.Id;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                throw;
            }
        }
    }
}
