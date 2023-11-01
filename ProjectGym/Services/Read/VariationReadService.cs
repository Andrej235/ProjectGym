using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class VariationReadService : AbstractReadService<ExerciseVariation, int>
    {
        protected override Func<ExerciseVariation, int> PrimaryKey => throw new NotImplementedException();

        protected override IQueryable<ExerciseVariation> GetIncluded(IEnumerable<string>? include)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<ExerciseVariation, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
