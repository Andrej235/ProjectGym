using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class SecondaryMuscleExerciseConnectionReadService : AbstractReadService<SecondaryMuscleGroupInExercise, int>
    {
        private readonly ExerciseContext context;
        public SecondaryMuscleExerciseConnectionReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<SecondaryMuscleGroupInExercise, int> PrimaryKey => x => x.Id;

        protected override IQueryable<SecondaryMuscleGroupInExercise> GetIncluded(IEnumerable<string>? include) => context.SecondaryMuscleGroups.AsQueryable();

        protected override Expression<Func<SecondaryMuscleGroupInExercise, bool>> TranslateKeyValueToExpression(string key, string value)
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
