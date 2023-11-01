using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class ExerciseVariationReadService : AbstractReadService<ExerciseVariation, int>
    {
        private readonly ExerciseContext context;
        public ExerciseVariationReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<ExerciseVariation, int> PrimaryKey => x => x.Id;

        protected override IQueryable<ExerciseVariation> GetIncluded(IEnumerable<string>? include) => context.ExerciseVariations.AsQueryable();

        protected override Expression<Func<ExerciseVariation, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (int.TryParse(value, out int valueId))
            {
                return key switch
                {
                    "exercise1" => x => x.Exercise1Id == valueId,
                    "exercise2" => x => x.Exercise2Id == valueId,
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}")
                };
            }
            throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
        }
    }
}
