using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class MuscleGroupReadService : AbstractReadService<MuscleGroup, int>
    {
        protected override Func<MuscleGroup, int> PrimaryKey => throw new NotImplementedException();

        protected override IQueryable<MuscleGroup> GetIncluded(IEnumerable<string>? include)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<MuscleGroup, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
