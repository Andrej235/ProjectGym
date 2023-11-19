using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class MuscleGroupReadService(ExerciseContext context) : AbstractReadService<MuscleGroup, int>
    {
        protected override Func<MuscleGroup, int> PrimaryKey => x => x.Id;

        protected override IQueryable<MuscleGroup> GetIncluded(IEnumerable<string>? include)
        {
            IQueryable<MuscleGroup> entitiesIncluding = context.MuscleGroups.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return entitiesIncluding;

            if (include.Contains("all"))
            {
                return entitiesIncluding
                    .Include(x => x.Muscles)
                    .Include(x => x.PrimaryInExercises)
                    .Include(x => x.SecondaryInExercises);
            }

            foreach (var inc in include)
            {
                entitiesIncluding = inc switch
                {
                    "muscles" => entitiesIncluding.Include(x => x.Muscles),
                    "primaryin" => entitiesIncluding.Include(x => x.PrimaryInExercises),
                    "secondaryin" => entitiesIncluding.Include(x => x.SecondaryInExercises),
                    _ => entitiesIncluding
                };
            }

            return entitiesIncluding;
        }

        protected override Expression<Func<MuscleGroup, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return m => m.Name.ToLower().Contains(value.ToLower());
            }
            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
