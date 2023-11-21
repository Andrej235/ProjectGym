using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class WeightReadService(ExerciseContext context) : AbstractReadService<PersonalExerciseWeight, Guid>
    {
        protected override Func<PersonalExerciseWeight, Guid> PrimaryKey => x => x.Id;

        protected override IQueryable<PersonalExerciseWeight> GetIncluded(IEnumerable<string>? include) => context.Weights.AsQueryable();

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
