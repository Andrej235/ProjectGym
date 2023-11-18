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

            if (include.Contains("all") || include.Contains("muscles"))
            {
                return entitiesIncluding
                    .Include(x => x.Muscles);
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
