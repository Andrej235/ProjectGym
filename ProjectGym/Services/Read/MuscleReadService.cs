using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class MuscleReadService(ExerciseContext context) : AbstractReadService<Muscle, int>
    {
        protected override Func<Muscle, int> PrimaryKey => m => m.Id;

        protected override IQueryable<Muscle> GetIncluded(IEnumerable<string>? include)
        {
            IQueryable<Muscle> entitiesIncluding = context.Muscles.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return entitiesIncluding;

            if (include.Contains("all") || include.Contains("musclegroup"))
            {
                return entitiesIncluding
                    .Include(x => x.MuscleGroup);
            }

            return entitiesIncluding;
        }

        protected override Expression<Func<Muscle, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return x => x.Name.ToLower().Contains(value.ToLower());
            }
            else if (key == "musclegroup")
            {
                if (int.TryParse(value, out var id))
                    return x => x.MuscleGroupId == id;
                else
                    throw new NotSupportedException($"Invalid value in search query. Entered value '{value}' for key '{key}'");
            }
            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
