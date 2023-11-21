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
            //TODO: Implement multiple values per search ---> like in exercise read service
            /*TODO: 
                -Read service for supersets
                -Update services for sets, workout sets and supersets
                -more ways to search sets and workouts (like I did for workout sets below)
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
                throw new NotSupportedException($"Invalid value in search query. Entered value '{value}' for key '{key}'");
            }
        }
    }
}
