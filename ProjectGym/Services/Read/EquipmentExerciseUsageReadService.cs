using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class EquipmentExerciseUsageReadService : AbstractReadService<EquipmentExerciseUsage, int>
    {
        private readonly ExerciseContext context;
        public EquipmentExerciseUsageReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<EquipmentExerciseUsage, int> PrimaryKey => x => x.Id;

        protected override IQueryable<EquipmentExerciseUsage> GetIncluded(IEnumerable<string>? include) => context.EquipmentExerciseUsages.AsQueryable();

        protected override Expression<Func<EquipmentExerciseUsage, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (int.TryParse(value, out int valueId))
            {
                return key switch
                {
                    "equipment" => x => x.EquipmentId == valueId,
                    "exercise" => x => x.ExerciseId == valueId,
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                };
            }
            throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
        }
    }
}
