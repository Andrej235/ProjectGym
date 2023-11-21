using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class SupersetReadService(ExerciseContext context) : AbstractReadService<Superset, Guid>
    {
        protected override Func<Superset, Guid> PrimaryKey => x => x.Id;

        protected override IQueryable<Superset> GetIncluded(IEnumerable<string>? include) => context.Supersets.AsQueryable();

        protected override Expression<Func<Superset, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
