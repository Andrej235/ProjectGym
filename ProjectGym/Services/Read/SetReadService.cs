using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class SetReadService(ExerciseContext context) : AbstractReadService<Set, Guid>
    {
        protected override Func<Set, Guid> PrimaryKey => x => x.Id;

        protected override IQueryable<Set> GetIncluded(IEnumerable<string>? include) => context.Sets.AsQueryable();

        protected override Expression<Func<Set, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (bool.TryParse(value, out bool boolValue))
            {
                return key switch
                {
                    "tofaliure" => x => x.ToFaliure == boolValue,
                    "partials" => x => x.ToFaliure == boolValue,
                    _ => throw new NotSupportedException($"Invalid value in search query. Entered value '{value}' for key '{key}'")
                };
            }
            else
            {
                throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
            }
        }
    }
}
