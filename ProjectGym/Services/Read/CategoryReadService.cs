using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class CategoryReadService : AbstractReadService<ExerciseCategory, int>
    {
        private readonly ExerciseContext context;
        public CategoryReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<ExerciseCategory, int> PrimaryKey => c => c.Id;

        protected override IQueryable<ExerciseCategory> GetIncluded(IEnumerable<string>? include)
        {
            var entitiesIncluding = context.ExerciseCategories.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return entitiesIncluding;

            if (include.Contains("all") || include.Contains("exercises"))
                return entitiesIncluding.Include(x => x.Exercises);

            return entitiesIncluding;
        }

        protected override Expression<Func<ExerciseCategory, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return x => x.Name.ToLower().Contains(value.ToLower());
            }
            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
