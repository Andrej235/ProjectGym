using ProjectGym.Data;
using ProjectGym.Exceptions;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
{
    public class AliasCreateService(ExerciseContext context, IReadService<Exercise> exerciseReadService) : CreateService<Alias>(context)
    {
        protected override async Task<Exception?> IsEntityValid(Alias entity)
        {
            try
            {
                if (entity.ExerciseId < 1)
                    return new EntityNotFoundException($"Exercise with Id {entity.ExerciseId} was not found");
                
                await exerciseReadService.Get(x => x.Id == entity.ExerciseId, "none");
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
