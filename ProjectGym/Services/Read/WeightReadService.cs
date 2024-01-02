using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class WeightReadService(ExerciseContext context) : ReadService<PersonalExerciseWeight>(context)
    {
        protected override List<PersonalExerciseWeight> ApplyOffsetAndLimit(IQueryable<PersonalExerciseWeight> queryable, int? offset = 0, int? limit = -1)
        {
            var baseRes = base.ApplyOffsetAndLimit(queryable.Reverse().AsQueryable(), offset, limit);
            baseRes.Reverse();
            return baseRes;
        }

        protected override Expression<Func<PersonalExerciseWeight, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "exercise" && int.TryParse(value, out var exerciseId))
                return x => x.ExerciseId == exerciseId;
            else if (key == "user" && Guid.TryParse(value, out var userId))
                return x => x.UserId == userId;
            else if ((key == "iscurrent" || key == "current") && bool.TryParse(value, out var isCurrent))
                return x => x.IsCurrent == isCurrent;

            throw new NotSupportedException($"Invalid search query key-value pair. Entered key: {key} | Entered value: {value}");
        }
    }
}
