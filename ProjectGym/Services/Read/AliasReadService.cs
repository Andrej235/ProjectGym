using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Mapping;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class AliasReadService : AbstractReadService<ExerciseAlias, int>
    {
        private readonly ExerciseContext context;
        public AliasReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<ExerciseAlias, int> PrimaryKey => a => a.Id;

        protected override IQueryable<ExerciseAlias> GetIncluded(IEnumerable<string>? include)
        {
            IQueryable<ExerciseAlias> entitiesIncluding = context.ExerciseAliases.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return entitiesIncluding;

            if (include.Contains("all") || include.Contains("exercise"))
                return entitiesIncluding.Include(x => x.Exercise);

            return entitiesIncluding;
        }

        protected override Expression<Func<ExerciseAlias, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return x => x.Alias.ToLower().Contains(value.ToLower());
            }
            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
