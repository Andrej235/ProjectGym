using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class PrimaryMuscleReadService(ExerciseContext context) : AbstractReadService<PrimaryMuscleInExercise, int>
    {
        protected override Func<PrimaryMuscleInExercise, int> PrimaryKey => x => x.Id;

        protected override IQueryable<PrimaryMuscleInExercise> GetIncluded(IEnumerable<string>? include) => context.PrimaryMuscles.AsQueryable();

        protected override Expression<Func<PrimaryMuscleInExercise, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (int.TryParse(value, out int valueId))
            {
                return key switch
                {
                    "muscle" => x => x.MuscleId == valueId,
                    "exercise" => x => x.ExerciseId == valueId,
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                };
            }
            throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
        }
    }
}
