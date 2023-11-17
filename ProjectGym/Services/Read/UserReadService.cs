using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class UserReadService : AbstractReadService<User, Guid>
    {
        private readonly ExerciseContext context;
        public UserReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<User, Guid> PrimaryKey => u => u.Id;

        protected override IQueryable<User> GetIncluded(IEnumerable<string>? include)
        {
            IQueryable<User> usersIncluding = context.Users.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return usersIncluding;

            if (include.Contains("all"))
                return usersIncluding
                    .Include(u => u.Weights)
                    .Include(u => u.CreatedWorkouts)
                    .Include(u => u.CreatedExerciseSets)
                    .Include(u => u.Comments)
                    .Include(u => u.CommentUpvotes)
                    .Include(u => u.CommentDownvotes)
                    .Include(u => u.Bookmarks);

            foreach (var inc in include)
            {
                usersIncluding = inc switch
                {
                    "weights" => usersIncluding.Include(u => u.Weights),
                    "workouts" => usersIncluding.Include(u => u.CreatedWorkouts),
                    "sets" => usersIncluding.Include(u => u.CreatedExerciseSets),
                    "comments" => usersIncluding.Include(u => u.Comments),
                    "upvotes" => usersIncluding.Include(u => u.CommentUpvotes),
                    "downvotes" => usersIncluding.Include(u => u.CommentDownvotes),
                    "bookmarks" => usersIncluding.Include(u => u.Bookmarks),
                    _ => usersIncluding,
                };
            }
            return usersIncluding;
        }

        protected override Expression<Func<User, bool>> TranslateKeyValueToExpression(string key, string value) => u => false;
    }
}
