using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
{
    public class PrimaryMuscleExerciseConnectionCreateService(ExerciseContext context, IReadService<PrimaryMuscleGroupInExercise> readService) : CreateService<PrimaryMuscleGroupInExercise>(context)
    {
        protected override async Task<Exception?> IsEntityValid(PrimaryMuscleGroupInExercise entity)
        {
            try
            {
                await readService.Get(x => x.MuscleGroupId == entity.MuscleGroupId && x.ExerciseId == entity.ExerciseId, "none");
                return new Exception("Entity already exists");
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
