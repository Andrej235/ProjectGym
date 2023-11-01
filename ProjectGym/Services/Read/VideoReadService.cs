using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class VideoReadService : AbstractReadService<ExerciseVideo, int>
    {
        private readonly ExerciseContext context;
        public VideoReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<ExerciseVideo, int> PrimaryKey => x => x.Id;

        protected override IQueryable<ExerciseVideo> GetIncluded(IEnumerable<string>? include)
        {
            var entitiesIncluding = context.ExerciseVideos.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return entitiesIncluding;

            if (include.Contains("all") || include.Contains("exercise"))
                return entitiesIncluding.Include(x => x.Exercise);

            return entitiesIncluding;
        }

        protected override Expression<Func<ExerciseVideo, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key != "exercise")
                throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");

            if (int.TryParse(value, out int id))
                return x => x.ExerciseId == id;

            throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
        }
    }
}
