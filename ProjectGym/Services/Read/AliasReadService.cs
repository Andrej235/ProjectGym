using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Mapping;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class AliasReadService(ExerciseContext context) : AbstractReadService<Alias, int>
    {
        protected override Func<Alias, int> PrimaryKey => a => a.Id;

        protected override IQueryable<Alias> GetIncluded(IEnumerable<string>? include)
        {
            IQueryable<Alias> entitiesIncluding = context.Aliases.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return entitiesIncluding;

            if (include.Contains("all") || include.Contains("exercise"))
                return entitiesIncluding.Include(x => x.Exercise);

            return entitiesIncluding;
        }

        protected override Expression<Func<Alias, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return x => x.AliasName.ToLower().Contains(value.ToLower());
            }
            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
