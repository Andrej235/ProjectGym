using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
{
    public class SecondaryMuscleExerciseConnectionCreateService(ExerciseContext context, IReadService<SecondaryMuscleGroupInExercise> readService) : CreateService<SecondaryMuscleGroupInExercise>(context)
    {
        protected override async Task<Exception?> IsEntityValid(SecondaryMuscleGroupInExercise entity)
        {
            try
            {
                await readService.Get(x => x.MuscleGroupId == entity.MuscleGroupId && x.ExerciseId == entity.ExerciseId, "none");
                throw new Exception("Entity already exists");
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
