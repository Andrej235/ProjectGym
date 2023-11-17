using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class NoteReadService : AbstractReadService<Note, int>
    {
        private readonly ExerciseContext context;
        public NoteReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<Note, int> PrimaryKey => n => n.Id;

        protected override IQueryable<Note> GetIncluded(IEnumerable<string>? include)
        {
            var entitiesIncluding = context.Notes.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return entitiesIncluding;

            if (include.Contains("all") || include.Contains("exercise"))
                return entitiesIncluding.Include(x => x.Exercise);

            return entitiesIncluding;
        }

        protected override Expression<Func<Note, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return x => x.NoteText.ToLower().Contains(value.ToLower());
            }
            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
