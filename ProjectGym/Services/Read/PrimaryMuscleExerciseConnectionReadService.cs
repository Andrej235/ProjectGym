using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class PrimaryMuscleExerciseConnectionReadService : AbstractReadService<PrimaryMuscleExerciseConnection, int>
    {
        private readonly ExerciseContext context;
        public PrimaryMuscleExerciseConnectionReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<PrimaryMuscleExerciseConnection, int> PrimaryKey => x => x.Id;

        protected override IQueryable<PrimaryMuscleExerciseConnection> GetIncluded(IEnumerable<string>? include) => context.PrimaryMuscleExerciseConnections.AsQueryable();

        protected override Expression<Func<PrimaryMuscleExerciseConnection, bool>> TranslateKeyValueToExpression(string key, string value)
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
