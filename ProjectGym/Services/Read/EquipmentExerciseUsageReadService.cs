using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class EquipmentExerciseUsageReadService(ExerciseContext context) : ReadService<EquipmentUsage>(context)
    {
        protected override Expression<Func<EquipmentUsage, bool>> TranslateKeyValueToExpression(string key, string value)
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
