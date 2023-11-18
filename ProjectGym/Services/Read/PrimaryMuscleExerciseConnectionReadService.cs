using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class PrimaryMuscleExerciseConnectionReadService(ExerciseContext context) : AbstractReadService<PrimaryMuscleGroupInExercise, int>
    {
        protected override Func<PrimaryMuscleGroupInExercise, int> PrimaryKey => x => x.Id;

        protected override IQueryable<PrimaryMuscleGroupInExercise> GetIncluded(IEnumerable<string>? include) => context.PrimaryMuscleGroups.AsQueryable();

        protected override Expression<Func<PrimaryMuscleGroupInExercise, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (int.TryParse(value, out int valueId))
            {
                return key switch
                {
                    "muscle" => x => x.MuscleGroupId == valueId,
                    "exercise" => x => x.ExerciseId == valueId,
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                };
            }
            throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
        }
    }
}
