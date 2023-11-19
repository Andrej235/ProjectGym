using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class WeightCreateService(ExerciseContext context,
                                     IReadService<User> userReadService,
                                     IReadService<PersonalExerciseWeight> weightReadService,
                                     IReadService<Exercise> exerciseReadService) : ICreateService<PersonalExerciseWeight, Guid>
    {
        public async Task<Guid> Add(PersonalExerciseWeight toAdd)
        {
            try
            {
                await userReadService.Get(x => x.Id == toAdd.UserId, "none");
                await exerciseReadService.Get(x => x.Id == toAdd.ExerciseId, "none");

                if (toAdd.IsCurrent)
                {
                    var allCurrentWeights = await weightReadService.Get(x => x.UserId == toAdd.UserId && x.ExerciseId == toAdd.ExerciseId && x.IsCurrent, 0, -1, "none");
                    //***********************************************************
                    //TODO: Replace with a update service
                    context.AttachRange(allCurrentWeights);
                    allCurrentWeights.ForEach(x => x.IsCurrent = false);
                    await context.SaveChangesAsync();
                    //***********************************************************
                }

                await context.Weights.AddAsync(toAdd);
                await context.SaveChangesAsync();
                return toAdd.Id;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return default;
            }
        }
    }
}
