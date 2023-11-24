using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class WorkoutSetReadService(ExerciseContext context) : AbstractReadService<WorkoutSet, Guid>
    {
        protected override Func<WorkoutSet, Guid> PrimaryKey => x => x.Id;

        protected override IQueryable<WorkoutSet> GetIncluded(IEnumerable<string>? include) => context.WorkoutSets.AsQueryable();

        protected override Expression<Func<WorkoutSet, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            /*TODO: 
                -Update services for sets and workout sets
             */
            if (int.TryParse(value, out int valueId))
            {
                return key switch
                {
                    "equipment" => x => x.Set.Exercise.Equipment.Any(x => x.Id == valueId),
                    "primarymusclegroup" => x => x.Set.Exercise.PrimaryMuscleGroups.Any(x => x.Id == valueId),
                    "secondarymusclegroup" => x => x.Set.Exercise.SecondaryMuscleGroups.Any(x => x.Id == valueId),
                    "primarymuscle" => x => x.Set.Exercise.PrimaryMuscles.Any(x => x.Id == valueId),
                    "secondarymuscle" => x => x.Set.Exercise.SecondaryMuscles.Any(x => x.Id == valueId),
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}")
                };
            }
            else
            {
                if (value.Contains(','))
                {
                    var values = value.Replace(" ", "").Split(',');
                    List<int> valueIds = [];
                    foreach (var id in values)
                    {
                        if (int.TryParse(id, out int newId))
                            valueIds.Add(newId);
                    }

                    return key switch
                    {
                        "primarymusclegroup" => x => x.Set.Exercise.PrimaryMuscleGroups.Any(m => valueIds.Contains(m.Id)),
                        "secondarymusclegroup" => x => x.Set.Exercise.SecondaryMuscleGroups.Any(m => valueIds.Contains(m.Id)),
                        "primarymuscle" => x => x.Set.Exercise.PrimaryMuscles.Any(m => valueIds.Contains(m.Id)),
                        "secondarymuscle" => x => x.Set.Exercise.SecondaryMuscles.Any(m => valueIds.Contains(m.Id)),
                        "equipment" => x => x.Set.Exercise.Equipment.Any(eq => valueIds.Contains(eq.Id)),
                        _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                    };
                }
                throw new NotSupportedException($"Invalid value in search query. Entered value '{value}' for key '{key}'");
            }
        }
    }
}
