using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class BookmarkReadService(ExerciseContext context) : AbstractReadService<ExerciseBookmark>
    {
        protected override IQueryable<ExerciseBookmark> GetIncluded(IEnumerable<string>? include) => context.ExerciseBookmarks.AsQueryable();

        protected override Expression<Func<ExerciseBookmark, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "exercise" && int.TryParse(value, out var exerciseId))
                return x => x.ExerciseId == exerciseId;
            else if (key == "user" && Guid.TryParse(value, out var userId))
                return x => x.UserId == userId;

            throw new NotSupportedException($"Invalid search query key-value pair. Entered key: {key} | Entered value: {value}");
        }
    }
}
